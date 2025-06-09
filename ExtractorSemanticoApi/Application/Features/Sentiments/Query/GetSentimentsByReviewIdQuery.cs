using ExtractorSemanticoApi.Application.Dto.Sentiments;
using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.Sentiments.Query;

public record GetSentimentsByReviewIdQuery(int ReviewId) : IRequest<List<SentimentResponseDto>>;

public class GetSentimentsByReviewIdQueryHandler : IRequestHandler<GetSentimentsByReviewIdQuery, List<SentimentResponseDto>>
{
    private readonly ISentimentRepository _sentimentRepository;

    public GetSentimentsByReviewIdQueryHandler(ISentimentRepository sentimentRepository)
    {
        _sentimentRepository = sentimentRepository;
    }

    public async Task<List<SentimentResponseDto>> Handle(
        GetSentimentsByReviewIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _sentimentRepository.GetSentimentsByReviewId(request.ReviewId);
    }
}