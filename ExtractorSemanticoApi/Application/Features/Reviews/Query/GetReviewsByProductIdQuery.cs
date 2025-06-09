using ExtractorSemanticoApi.Application.Dto.Reviews;
using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.Reviews.Query;
public record GetReviewsByProductIdQuery(int ProductId) : IRequest<List<ReviewResponseDto>>;

public class GetReviewsByProductIdQueryHandler : IRequestHandler<GetReviewsByProductIdQuery, List<ReviewResponseDto>>
{
    private readonly IReviewRepository _reviewRepository;

    public GetReviewsByProductIdQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<List<ReviewResponseDto>> Handle(
        GetReviewsByProductIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _reviewRepository.GetReviewsByProductId(request.ProductId);
    }
}