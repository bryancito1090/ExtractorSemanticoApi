using ExtractorSemanticoApi.Application.Dto.Reviews;
using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.Reviews.Command;

public record CreateUpdateReviewCommand(
    int? ReviewId,
    int ProductId,
    string UserName,
    string OriginalText,
    string? CleanText,
    int? Rating,
    DateTime? ReviewDate,
    string? Location
) : IRequest<ReviewResponseDto>;

public class CreateUpdateReviewCommandHandler : IRequestHandler<CreateUpdateReviewCommand, ReviewResponseDto>
{
    private readonly IReviewRepository _reviewRepository;

    public CreateUpdateReviewCommandHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<ReviewResponseDto> Handle(
        CreateUpdateReviewCommand request,
        CancellationToken cancellationToken)
    {
        var reviewDto = new ReviewRequestDto(
            request.ReviewId,
            request.ProductId,
            request.UserName,
            request.OriginalText,
            request.CleanText,
            request.Rating,
            request.ReviewDate,
            request.Location
        );

        return await _reviewRepository.CreateUpdateReview(reviewDto);
    }
}