using ExtractorSemanticoApi.Application.Dto.Products;
using ExtractorSemanticoApi.Application.Dto.Reviews;
using ExtractorSemanticoApi.Application.Interfaces;
using ExtractorSemanticoApi.Dominio;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExtractorSemanticoApi.Persistencia.Repository;

public class ReviewRepository : IReviewRepository
{
    private readonly ExtractorsemanticoContext _context;

    public ReviewRepository(ExtractorsemanticoContext context)
    {
        _context = context;
    }

    public async Task<ReviewResponseDto> CreateUpdateReview(ReviewRequestDto request)
    {
        Review entity;

        if (request.ReviewId.HasValue && request.ReviewId > 0)
        {
            // Update existing review
            entity = await _context.Reviews
                .FirstOrDefaultAsync(r => r.ReviewId == request.ReviewId)
                ?? throw new KeyNotFoundException($"Review with ID {request.ReviewId} not found");

            entity.ProductId = request.ProductId;
            entity.UserName = request.UserName;
            entity.OriginalText = request.OriginalText;
            entity.CleanText = request.CleanText;
            entity.Rating = request.Rating;
            entity.ReviewDate = request.ReviewDate;
            entity.Location = request.Location;

            _context.Reviews.Update(entity);
        }
        else
        {
            // Create new review
            entity = new Review
            {
                ProductId = request.ProductId,
                UserName = request.UserName,
                OriginalText = request.OriginalText,
                CleanText = request.CleanText,
                Rating = request.Rating,
                ReviewDate = request.ReviewDate,
                Location = request.Location
            };

            await _context.Reviews.AddAsync(entity);
        }

        await _context.SaveChangesAsync();

        return new ReviewResponseDto(
            entity.ReviewId,
            entity.ProductId,
            entity.UserName,
            entity.OriginalText,
            entity.CleanText,
            entity.Rating,
            entity.ReviewDate,
            entity.Location
        );
    }

    public async Task<List<ReviewResponseDto>> GetReviews()
    {
        return await _context.Reviews
            .Select(r => new ReviewResponseDto(
                r.ReviewId,
                r.ProductId,
                r.UserName,
                r.OriginalText,
                r.CleanText,
                r.Rating,
                r.ReviewDate,
                r.Location))
            .ToListAsync();
    }

    public async Task<List<ReviewResponseDto>> GetReviewsByProductId(int productId)
    {
        return await _context.Reviews
            .Where(r => r.ProductId == productId)
            .Select(r => new ReviewResponseDto(
                r.ReviewId,
                r.ProductId,
                r.UserName,
                r.OriginalText,
                r.CleanText,
                r.Rating,
                r.ReviewDate,
                r.Location))
            .ToListAsync();
    }

    public async Task<ReviewResponseDto> GetReviewById(int id)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(r => r.ReviewId == id)
            ?? throw new KeyNotFoundException($"Review with ID {id} not found");

        return new ReviewResponseDto(
            review.ReviewId,
            review.ProductId,
            review.UserName,
            review.OriginalText,
            review.CleanText,
            review.Rating,
            review.ReviewDate,
            review.Location);
    }

    public async Task UpdateReview(ReviewResponseDto reviewDto)
    {
        var review = await _context.Reviews
            .FirstOrDefaultAsync(r => r.ReviewId == reviewDto.ReviewId)
            ?? throw new KeyNotFoundException($"Review with ID {reviewDto.ReviewId} not found");

        review.CleanText = reviewDto.CleanText;
        review.Rating = reviewDto.Rating;
        review.ReviewDate = reviewDto.ReviewDate;
        review.Location = reviewDto.Location;

        _context.Reviews.Update(review);
        await _context.SaveChangesAsync();
    }
}