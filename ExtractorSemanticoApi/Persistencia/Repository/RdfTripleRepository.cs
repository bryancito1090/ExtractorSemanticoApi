using ExtractorSemanticoApi.Application.Dto.Products;
using ExtractorSemanticoApi.Application.Dto.RdfTriples;
using ExtractorSemanticoApi.Application.Dto.Reviews;
using ExtractorSemanticoApi.Application.Interfaces;
using ExtractorSemanticoApi.Dominio;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExtractorSemanticoApi.Persistencia.Repository;

public class RdfTripleRepository : IRdfTripleRepository
{
    private readonly ExtractorsemanticoContext _context;
    private readonly IReviewRepository _reviewRepository;
    private readonly IExtractedDataRepository _extractedDataRepository;
    private readonly ISentimentRepository _sentimentRepository;

    public RdfTripleRepository(
        ExtractorsemanticoContext context,
        IReviewRepository reviewRepository,
        IExtractedDataRepository extractedDataRepository,
        ISentimentRepository sentimentRepository)
    {
        _context = context;
        _reviewRepository = reviewRepository;
        _extractedDataRepository = extractedDataRepository;
        _sentimentRepository = sentimentRepository;
    }

    public async Task<RdfTripleResponseDto> CreateUpdateRdfTriple(RdfTripleRequestDto request)
    {
        RdfTriple entity;

        if (request.TripleId.HasValue && request.TripleId > 0)
        {
            // Update existing triple
            entity = await _context.RdfTriples
                .FirstOrDefaultAsync(rt => rt.TripleId == request.TripleId)
                ?? throw new KeyNotFoundException($"RdfTriple with ID {request.TripleId} not found");

            entity.Subject = request.Subject;
            entity.Predicate = request.Predicate;
            entity.Object = request.Object;
            entity.SourceReviewId = request.SourceReviewId;

            _context.RdfTriples.Update(entity);
        }
        else
        {
            // Create new triple
            entity = new RdfTriple
            {
                Subject = request.Subject,
                Predicate = request.Predicate,
                Object = request.Object,
                SourceReviewId = request.SourceReviewId
            };

            await _context.RdfTriples.AddAsync(entity);
        }

        await _context.SaveChangesAsync();

        return new RdfTripleResponseDto(
            entity.TripleId,
            entity.Subject,
            entity.Predicate,
            entity.Object,
            entity.SourceReviewId
        );
    }

    public async Task<List<RdfTripleResponseDto>> GetRdfTriples()
    {
        return await _context.RdfTriples
            .Select(rt => new RdfTripleResponseDto(
                rt.TripleId,
                rt.Subject,
                rt.Predicate,
                rt.Object,
                rt.SourceReviewId))
            .ToListAsync();
    }

    public async Task<List<RdfTripleResponseDto>> GetRdfTriplesByReviewId(int reviewId)
    {
        return await _context.RdfTriples
            .Where(rt => rt.SourceReviewId == reviewId)
            .Select(rt => new RdfTripleResponseDto(
                rt.TripleId,
                rt.Subject,
                rt.Predicate,
                rt.Object,
                rt.SourceReviewId))
            .ToListAsync();
    }

    public async Task GenerateRdfTriplesFromReview(int reviewId)
    {
        // Primero eliminamos las tripletas existentes para este review
        await DeleteTriplesByReviewId(reviewId);

        var review = await _reviewRepository.GetReviewById(reviewId);
        var product = await _context.Products.FindAsync(review.ProductId);

        // Triple básica: (usuario, escribió_reseña, producto)
        await CreateUpdateRdfTriple(new RdfTripleRequestDto(
            null,
            review.UserName,
            "escribió_reseña",
            product?.Name ?? "producto_desconocido",
            reviewId
        ));

        // Triple para rating si existe
        if (review.Rating.HasValue)
        {
            await CreateUpdateRdfTriple(new RdfTripleRequestDto(
                null,
                review.UserName,
                "calificó_con",
                review.Rating.Value.ToString(),
                reviewId
            ));
        }

        // Triples para sentimientos
        var sentiments = await _sentimentRepository.GetSentimentsByReviewId(reviewId);
        foreach (var sentiment in sentiments)
        {
            await CreateUpdateRdfTriple(new RdfTripleRequestDto(
                null,
                review.UserName,
                $"tiene_opinion_{sentiment.SentimentType.ToLower()}_sobre",
                sentiment.Aspect,
                reviewId
            ));
        }

        // Triples para eventos extraídos
        var extractedData = await _extractedDataRepository.GetExtractedDataByReviewId(reviewId);
        var events = extractedData.Where(ed => ed.Type == "EVENT");

        foreach (var ev in events)
        {
            await CreateUpdateRdfTriple(new RdfTripleRequestDto(
                null,
                review.UserName,
                ev.Value,
                product?.Name ?? "producto_desconocido",
                reviewId
            ));
        }
    }

    public async Task DeleteTriplesByReviewId(int reviewId)
    {
        var triplesToDelete = await _context.RdfTriples
            .Where(rt => rt.SourceReviewId == reviewId)
            .ToListAsync();

        _context.RdfTriples.RemoveRange(triplesToDelete);
        await _context.SaveChangesAsync();
    }
}