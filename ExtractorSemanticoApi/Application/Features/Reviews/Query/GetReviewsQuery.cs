using ExtractorSemanticoApi.Application.Dto.Reviews;
using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.Reviews.Query;


public record GetReviewsQuery : IRequest<List<ReviewResponseDto>>;

public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, List<ReviewResponseDto>>
{
    private readonly IReviewRepository _reviewRepository;

    public GetReviewsQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<List<ReviewResponseDto>> Handle(
        GetReviewsQuery request,
        CancellationToken cancellationToken)
    {
        return await _reviewRepository.GetReviews();
    }
}