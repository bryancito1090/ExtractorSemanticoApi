using ExtractorSemanticoApi.Application.Dto.RdfTriples;
using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.RdfTriples.Query;


public record GetRdfTriplesQuery : IRequest<List<RdfTripleResponseDto>>;

public class GetRdfTriplesQueryHandler : IRequestHandler<GetRdfTriplesQuery, List<RdfTripleResponseDto>>
{
    private readonly IRdfTripleRepository _rdfTripleRepository;

    public GetRdfTriplesQueryHandler(IRdfTripleRepository rdfTripleRepository)
    {
        _rdfTripleRepository = rdfTripleRepository;
    }

    public async Task<List<RdfTripleResponseDto>> Handle(
        GetRdfTriplesQuery request,
        CancellationToken cancellationToken)
    {
        return await _rdfTripleRepository.GetRdfTriples();
    }
}