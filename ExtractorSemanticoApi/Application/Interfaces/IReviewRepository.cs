using ExtractorSemanticoApi.Application.Dto.Reviews;

namespace ExtractorSemanticoApi.Application.Interfaces;

public interface IReviewRepository
{
    Task<ReviewResponseDto> CreateUpdateReview(ReviewRequestDto request);
    Task<List<ReviewResponseDto>> GetReviews();
    Task<List<ReviewResponseDto>> GetReviewsByProductId(int productId);
    Task<ReviewResponseDto> GetReviewById(int id);
    Task UpdateReview(ReviewResponseDto review);
    // ELIMINAR: Task ProcessReviewText(int reviewId);
}