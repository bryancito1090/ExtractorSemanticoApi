using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.RdfTriples.Command;
public record DeleteRdfTriplesCommand(int ReviewId) : IRequest<Unit>;

public class DeleteRdfTriplesCommandHandler : IRequestHandler<DeleteRdfTriplesCommand, Unit>
{
    private readonly IRdfTripleRepository _rdfTripleRepository;

    public DeleteRdfTriplesCommandHandler(IRdfTripleRepository rdfTripleRepository)
    {
        _rdfTripleRepository = rdfTripleRepository;
    }

    public async Task<Unit> Handle(
        DeleteRdfTriplesCommand request,
        CancellationToken cancellationToken)
    {
        await _rdfTripleRepository.DeleteTriplesByReviewId(request.ReviewId);
        return Unit.Value;
    }
}