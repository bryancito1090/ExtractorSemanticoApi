using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.TextProcessing.Commands;

public record AnalyzeReviewSentimentsCommand(int ReviewId) : IRequest<Unit>;

public class AnalyzeReviewSentimentsCommandHandler : IRequestHandler<AnalyzeReviewSentimentsCommand, Unit>
{
    private readonly ITextProcessingService _textProcessingService;

    public AnalyzeReviewSentimentsCommandHandler(ITextProcessingService textProcessingService)
    {
        _textProcessingService = textProcessingService;
    }

    public async Task<Unit> Handle(AnalyzeReviewSentimentsCommand request, CancellationToken cancellationToken)
    {
        await _textProcessingService.AnalyzeReviewSentiments(request.ReviewId);
        return Unit.Value;
    }
}