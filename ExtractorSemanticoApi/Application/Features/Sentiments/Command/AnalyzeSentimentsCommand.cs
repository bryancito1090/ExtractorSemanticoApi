using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.Sentiments.Command;

public record AnalyzeSentimentsCommand(int ReviewId) : IRequest<Unit>;

public class AnalyzeSentimentsCommandHandler : IRequestHandler<AnalyzeSentimentsCommand, Unit>
{
    private readonly ISentimentRepository _sentimentRepository;

    public AnalyzeSentimentsCommandHandler(ISentimentRepository sentimentRepository)
    {
        _sentimentRepository = sentimentRepository;
    }

    public async Task<Unit> Handle(
        AnalyzeSentimentsCommand request,
        CancellationToken cancellationToken)
    {
        await _sentimentRepository.AnalyzeReviewSentiments(request.ReviewId);
        return Unit.Value;
    }
}