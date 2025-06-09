using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.TextProcessing.Commands;

public record ProcessReviewTextCommand(int ReviewId) : IRequest<Unit>;

public class ProcessReviewTextCommandHandler : IRequestHandler<ProcessReviewTextCommand, Unit>
{
    private readonly ITextProcessingService _textProcessingService;

    public ProcessReviewTextCommandHandler(ITextProcessingService textProcessingService)
    {
        _textProcessingService = textProcessingService;
    }

    public async Task<Unit> Handle(ProcessReviewTextCommand request, CancellationToken cancellationToken)
    {
        await _textProcessingService.ProcessReviewText(request.ReviewId);
        return Unit.Value;
    }
}