using ExtractorSemanticoApi.Application.Dto.RdfTriples;
using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.RdfTriples.Query;


public record GetRdfTriplesByReviewIdQuery(int ReviewId) : IRequest<List<RdfTripleResponseDto>>;

public class GetRdfTriplesByReviewIdQueryHandler : IRequestHandler<GetRdfTriplesByReviewIdQuery, List<RdfTripleResponseDto>>
{
    private readonly IRdfTripleRepository _rdfTripleRepository;

    public GetRdfTriplesByReviewIdQueryHandler(IRdfTripleRepository rdfTripleRepository)
    {
        _rdfTripleRepository = rdfTripleRepository;
    }

    public async Task<List<RdfTripleResponseDto>> Handle(
        GetRdfTriplesByReviewIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _rdfTripleRepository.GetRdfTriplesByReviewId(request.ReviewId);
    }
}