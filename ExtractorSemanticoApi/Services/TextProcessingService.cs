using Azure.AI.TextAnalytics;
using Azure;
using ExtractorSemanticoApi.Application.Interfaces;
using ExtractorSemanticoApi.Dominio;
using System.Text.RegularExpressions;
using System;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace ExtractorSemanticoApi.Services;

public class TextProcessingService : ITextProcessingService
{
    private readonly IConfiguration _configuration;
    private readonly ExtractorsemanticoContext _context;
    private readonly ISentimentRepository _sentimentRepository;
    private readonly IExtractedDataRepository _extractedDataRepository;
    private readonly IRdfTripleRepository _rdfTripleRepository;
    private readonly ILogger<TextProcessingService> _logger;
    private readonly EntityNormalizer _entityNormalizer = new EntityNormalizer();

    public TextProcessingService(
        IConfiguration configuration,
        ExtractorsemanticoContext context,
        ISentimentRepository sentimentRepository,
        IExtractedDataRepository extractedDataRepository,
        IRdfTripleRepository rdfTripleRepository,
        ILogger<TextProcessingService> logger)
    {
        _configuration = configuration;
        _context = context;
        _sentimentRepository = sentimentRepository;
        _extractedDataRepository = extractedDataRepository;
        _rdfTripleRepository = rdfTripleRepository;
        _logger = logger;
    }

    public async Task ProcessReviewText(int reviewId)
    {
        try
        {
            _logger.LogInformation($"Iniciando procesamiento de texto para review ID: {reviewId}");

            // Obtener la entidad Review directamente del contexto
            var review = await _context.Reviews.FindAsync(reviewId);
            if (review == null)
                throw new KeyNotFoundException($"Review with ID {reviewId} not found");

            // 1. Limpieza del texto
            review.CleanText = CleanText(review.OriginalText);
            _context.Reviews.Update(review);
            await _context.SaveChangesAsync();

            // 2. Extracción de entidades
            await ExtractEntitiesWithNER(review);

            // 3. Análisis de sentimientos
            await _sentimentRepository.AnalyzeReviewSentiments(reviewId);

            // 4. Generación de tripletas RDF
            await _rdfTripleRepository.GenerateRdfTriplesFromReview(reviewId);

            _logger.LogInformation($"Procesamiento completado para review ID: {reviewId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error procesando review ID: {reviewId}");
            throw;
        }
    }

    public async Task AnalyzeReviewSentiments(int reviewId)
    {
        try
        {
            _logger.LogInformation($"Iniciando análisis de sentimientos para review ID: {reviewId}");
            await _sentimentRepository.AnalyzeReviewSentiments(reviewId);
            _logger.LogInformation($"Análisis de sentimientos completado para review ID: {reviewId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error analizando sentimientos para review ID: {reviewId}");
            throw;
        }
    }

    public async Task GenerateRdfTriplesFromReview(int reviewId)
    {
        try
        {
            _logger.LogInformation($"Generando tripletas RDF para review ID: {reviewId}");
            await _rdfTripleRepository.GenerateRdfTriplesFromReview(reviewId);
            _logger.LogInformation($"Tripletas RDF generadas para review ID: {reviewId}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error generando tripletas RDF para review ID: {reviewId}");
            throw;
        }
    }

    private string CleanText(string text)
    {
        // Eliminar HTML tags
        var cleanText = Regex.Replace(text, "<.*?>", string.Empty);
        // Eliminar caracteres especiales no deseados
        cleanText = Regex.Replace(cleanText, @"[^\w\s.,;:!?()$€¥£-]", " ");
        // Normalizar espacios
        cleanText = Regex.Replace(cleanText, @"\s+", " ").Trim();
        return cleanText;
    }

    private async Task ExtractEntitiesWithNER(Review review)
    {
        try
        {
            var client = new TextAnalyticsClient(
                new Uri(_configuration["AzureTextAnalytics:Endpoint"]),
                new AzureKeyCredential(_configuration["AzureTextAnalytics:Key"]));

            // Eliminar datos extraídos existentes
            var existingData = await _context.ExtractedData
                .Where(ed => ed.ReviewId == review.ReviewId)
                .ToListAsync();
            _context.ExtractedData.RemoveRange(existingData);
            await _context.SaveChangesAsync();

            // 1. Extraer entidades con Azure Text Analytics
            var response = await client.RecognizeEntitiesAsync(review.CleanText ?? review.OriginalText);

            foreach (var entity in response.Value)
            {
                string normalizedValue = _entityNormalizer.NormalizeEntity(entity.Text);

                string subtype = entity.Category.ToString() switch
                {
                    "Person" => "PERSONA",
                    "Location" => "UBICACION",
                    "Organization" => "MARCA",
                    "Product" => "PRODUCTO",
                    "DateTime" => "FECHA",
                    "Quantity" => "PRECIO",
                    _ => "OTRO"
                };

                var extractedData = new ExtractedDatum
                {
                    ReviewId = review.ReviewId,
                    Type = "ENTITY",
                    Value = normalizedValue, // Aquí usas el valor normalizado
                    Subtype = subtype,
                    Metadata = JsonConvert.SerializeObject(new
                    {
                        confidence = entity.ConfidenceScore,
                        category = entity.Category.ToString(),
                        subCategory = entity.SubCategory ?? ""
                    })
                };

                await _context.ExtractedData.AddAsync(extractedData);
            }

            // 2. Extraer precios con regex
            ExtractPrices(review);

            // 3. Extraer fechas con regex
            ExtractDates(review);

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error extrayendo entidades para review ID: {review.ReviewId}");
            throw;
        }
    }

    private void ExtractPrices(Review review)
    {
        var pricePatterns = new Dictionary<string, string>
        {
            { @"\$\d+(?:\.\d{1,2})?", "USD" },
            { @"\d+\s*€", "EUR" },
            { @"\d+\s*£", "GBP" },
            { @"\d+\s*¥", "JPY" },
            { @"\d+\s*MXN", "MXN" }
        };

        foreach (var pattern in pricePatterns)
        {
            var matches = Regex.Matches(review.OriginalText, pattern.Key);
            foreach (Match match in matches)
            {
                var extractedData = new ExtractedDatum
                {
                    ReviewId = review.ReviewId,
                    Type = "ENTITY",
                    Value = match.Value,
                    Subtype = "PRECIO",
                    Metadata = JsonConvert.SerializeObject(new { currency = pattern.Value })
                };

                _context.ExtractedData.Add(extractedData);
            }
        }
    }

    private void ExtractDates(Review review)
    {
        var datePatterns = new[]
        {
            @"\d{1,2}/\d{1,2}/\d{2,4}",
            @"\d{1,2}-\d{1,2}-\d{2,4}",
            @"\d{1,2}\s+(?:ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic)[a-z]*\s+\d{2,4}",
            @"(?:enero|febrero|marzo|abril|mayo|junio|julio|agosto|septiembre|octubre|noviembre|diciembre)\s+\d{1,2},\s+\d{4}"
        };

        foreach (var pattern in datePatterns)
        {
            var matches = Regex.Matches(review.OriginalText, pattern, RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                var extractedData = new ExtractedDatum
                {
                    ReviewId = review.ReviewId,
                    Type = "ENTITY",
                    Value = match.Value,
                    Subtype = "FECHA"
                };

                _context.ExtractedData.Add(extractedData);
            }
        }
    }
}
public class EntityNormalizer
{
    private readonly Dictionary<string, string> _normalizations = new()
    {
        {"celular", "smartphone"},
        {"teléfono", "smartphone"},
        {"móvil", "smartphone"},
        {"cell phone", "smartphone"},
        {"batería", "battery"},
        {"pila", "battery"}
    };

    public string NormalizeEntity(string entity)
    {
        var lower = entity.ToLower();
        return _normalizations.ContainsKey(lower)
            ? _normalizations[lower]
            : entity;
    }
}
