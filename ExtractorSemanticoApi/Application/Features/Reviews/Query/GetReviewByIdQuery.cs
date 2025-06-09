using ExtractorSemanticoApi.Application.Dto.Reviews;
using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.Reviews.Query;

public record GetReviewByIdQuery(int Id) : IRequest<ReviewResponseDto>;

public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, ReviewResponseDto>
{
    private readonly IReviewRepository _reviewRepository;

    public GetReviewByIdQueryHandler(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<ReviewResponseDto> Handle(
        GetReviewByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _reviewRepository.GetReviewById(request.Id);
    }
}