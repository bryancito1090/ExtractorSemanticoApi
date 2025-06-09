using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.RdfTriples.Command;
public record GenerateRdfTriplesCommand(int ReviewId) : IRequest<Unit>;

public class GenerateRdfTriplesCommandHandler : IRequestHandler<GenerateRdfTriplesCommand, Unit>
{
    private readonly IRdfTripleRepository _rdfTripleRepository;

    public GenerateRdfTriplesCommandHandler(IRdfTripleRepository rdfTripleRepository)
    {
        _rdfTripleRepository = rdfTripleRepository;
    }

    public async Task<Unit> Handle(
        GenerateRdfTriplesCommand request,
        CancellationToken cancellationToken)
    {
        await _rdfTripleRepository.GenerateRdfTriplesFromReview(request.ReviewId);
        return Unit.Value;
    }
}