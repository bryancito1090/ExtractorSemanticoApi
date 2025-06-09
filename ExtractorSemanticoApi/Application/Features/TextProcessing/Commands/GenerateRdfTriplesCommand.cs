using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.TextProcessing.Commands;

public record GenerateRdfTriplesCommand(int ReviewId) : IRequest<Unit>;

public class GenerateRdfTriplesCommandHandler : IRequestHandler<GenerateRdfTriplesCommand, Unit>
{
    private readonly ITextProcessingService _textProcessingService;

    public GenerateRdfTriplesCommandHandler(ITextProcessingService textProcessingService)
    {
        _textProcessingService = textProcessingService;
    }

    public async Task<Unit> Handle(GenerateRdfTriplesCommand request, CancellationToken cancellationToken)
    {
        await _textProcessingService.GenerateRdfTriplesFromReview(request.ReviewId);
        return Unit.Value;
    }
}