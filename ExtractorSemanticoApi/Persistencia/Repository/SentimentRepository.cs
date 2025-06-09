using Azure.AI.TextAnalytics;
using ExtractorSemanticoApi.Application.Dto.Products;
using ExtractorSemanticoApi.Application.Dto.Reviews;
using ExtractorSemanticoApi.Application.Dto.Sentiments;
using ExtractorSemanticoApi.Application.Interfaces;
using ExtractorSemanticoApi.Dominio;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.RegularExpressions;
using Azure.AI.TextAnalytics; 
using Azure; // 
using System.Text.RegularExpressions;

namespace ExtractorSemanticoApi.Persistencia.Repository;
public class SentimentRepository : ISentimentRepository
{
    private readonly ExtractorsemanticoContext _context;
    private readonly IConfiguration _configuration;

    public SentimentRepository(
        ExtractorsemanticoContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<SentimentResponseDto> CreateUpdateSentiment(SentimentRequestDto request)
    {
        Sentiment entity;

        if (request.SentimentId.HasValue && request.SentimentId > 0)
        {
            entity = await _context.Sentiments
                .FirstOrDefaultAsync(s => s.SentimentId == request.SentimentId)
                ?? throw new KeyNotFoundException($"Sentiment with ID {request.SentimentId} not found");

            entity.Aspect = request.Aspect;
            entity.SentimentType = request.SentimentType;
            entity.Confidence = request.Confidence;

            _context.Sentiments.Update(entity);
        }
        else
        {
            entity = new Sentiment
            {
                ReviewId = request.ReviewId,
                Aspect = request.Aspect,
                SentimentType = request.SentimentType,
                Confidence = request.Confidence
            };

            await _context.Sentiments.AddAsync(entity);
        }

        await _context.SaveChangesAsync();

        return new SentimentResponseDto(
            entity.SentimentId,
            entity.ReviewId,
            entity.Aspect,
            entity.SentimentType,
            entity.Confidence
        );
    }

    public async Task<List<SentimentResponseDto>> GetSentimentsByReviewId(int reviewId)
    {
        return await _context.Sentiments
            .Where(s => s.ReviewId == reviewId)
            .Select(s => new SentimentResponseDto(
                s.SentimentId,
                s.ReviewId,
                s.Aspect,
                s.SentimentType,
                s.Confidence
            ))
            .ToListAsync();
    }

    public async Task AnalyzeReviewSentiments(int reviewId)
    {
        var review = await _context.Reviews
            .Include(r => r.Product)
            .FirstOrDefaultAsync(r => r.ReviewId == reviewId)
            ?? throw new KeyNotFoundException($"Review with ID {reviewId} not found");

        // Eliminar sentimientos existentes
        await DeleteSentimentsByReviewId(reviewId);

        var client = new TextAnalyticsClient(
            new Uri(_configuration["AzureTextAnalytics:Endpoint"]),
            new AzureKeyCredential(_configuration["AzureTextAnalytics:Key"]));

        // Análisis de sentimiento general
        var sentimentResponse = await client.AnalyzeSentimentAsync(review.CleanText ?? review.OriginalText);

        // Sustituye 'sentenceResponse' por 'sentimentResponse' en la creación de 'generalSentiment'
        var generalSentiment = new Sentiment
        {
            ReviewId = reviewId,
            Aspect = "general",
            SentimentType = MapSentiment(sentimentResponse.Value.Sentiment),
            Confidence = sentimentResponse.Value.Sentiment switch
            {
                TextSentiment.Positive => (float)sentimentResponse.Value.ConfidenceScores.Positive,
                TextSentiment.Negative => (float)sentimentResponse.Value.ConfidenceScores.Negative,
                _ => (float)sentimentResponse.Value.ConfidenceScores.Neutral
            }
        };
        await _context.Sentiments.AddAsync(generalSentiment);


        // Análisis por oraciones/frases
        var sentences = Regex.Split(review.CleanText ?? review.OriginalText, @"(?<=[.!?])\s+");

        foreach (var sentence in sentences.Where(s => !string.IsNullOrWhiteSpace(s)))
        {
            var sentenceResponse = await client.AnalyzeSentimentAsync(sentence);
            var sentiment = MapSentiment(sentenceResponse.Value.Sentiment);

            if (sentiment != "NEUTRAL")
            {
                var aspect = ExtractMainAspect(sentence);

                var newSentiment = new Sentiment
                {
                    ReviewId = reviewId,
                    Aspect = aspect ?? "general",
                    SentimentType = sentiment,

                    Confidence = sentenceResponse.Value.Sentiment switch
                    {
                        TextSentiment.Positive => (float)sentenceResponse.Value.ConfidenceScores.Positive,
                        TextSentiment.Negative => (float)sentenceResponse.Value.ConfidenceScores.Negative,
                        _ => (float)sentenceResponse.Value.ConfidenceScores.Neutral
                    }
                };
                await _context.Sentiments.AddAsync(newSentiment);
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task DeleteSentimentsByReviewId(int reviewId)
    {
        var sentiments = await _context.Sentiments
            .Where(s => s.ReviewId == reviewId)
            .ToListAsync();

        _context.Sentiments.RemoveRange(sentiments);
        await _context.SaveChangesAsync();
    }

    private string MapSentiment(TextSentiment sentiment)
    {
        return sentiment switch
        {
            TextSentiment.Positive => "POSITIVO",
            TextSentiment.Negative => "NEGATIVO",
            _ => "NEUTRAL"
        };
    }

    private string? ExtractMainAspect(string sentence)
    {
        // Implementación simplificada - identificar el sustantivo principal
        var words = sentence.Split(' ');
        var nouns = words.Where(w => w.Length > 3 && char.IsUpper(w[0]) && !IsVerb(w));
        return nouns.FirstOrDefault()?.ToLower();
    }

    private bool IsVerb(string word)
    {
        // Lista básica de verbos comunes en español
        var verbs = new[] { "es", "son", "tiene", "tienen", "fue", "fueron", "hace", "hacen" };
        return verbs.Contains(word.ToLower());
    }
}