using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.Sentiments.Command;


public record DeleteSentimentsCommand(int ReviewId) : IRequest<Unit>;

public class DeleteSentimentsCommandHandler : IRequestHandler<DeleteSentimentsCommand, Unit>
{
    private readonly ISentimentRepository _sentimentRepository;

    public DeleteSentimentsCommandHandler(ISentimentRepository sentimentRepository)
    {
        _sentimentRepository = sentimentRepository;
    }

    public async Task<Unit> Handle(
        DeleteSentimentsCommand request,
        CancellationToken cancellationToken)
    {
        await _sentimentRepository.DeleteSentimentsByReviewId(request.ReviewId);
        return Unit.Value;
    }
}