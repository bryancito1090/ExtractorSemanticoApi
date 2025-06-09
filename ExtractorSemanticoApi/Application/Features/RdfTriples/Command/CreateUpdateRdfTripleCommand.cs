using ExtractorSemanticoApi.Application.Dto.RdfTriples;
using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.RdfTriples.Command;

public record CreateUpdateRdfTripleCommand(
    int? TripleId,
    string Subject,
    string Predicate,
    string Object,
    int? SourceReviewId
) : IRequest<RdfTripleResponseDto>;

public class CreateUpdateRdfTripleCommandHandler : IRequestHandler<CreateUpdateRdfTripleCommand, RdfTripleResponseDto>
{
    private readonly IRdfTripleRepository _rdfTripleRepository;

    public CreateUpdateRdfTripleCommandHandler(IRdfTripleRepository rdfTripleRepository)
    {
        _rdfTripleRepository = rdfTripleRepository;
    }

    public async Task<RdfTripleResponseDto> Handle(
        CreateUpdateRdfTripleCommand request,
        CancellationToken cancellationToken)
    {
        var rdfTripleDto = new RdfTripleRequestDto(
            request.TripleId,
            request.Subject,
            request.Predicate,
            request.Object,
            request.SourceReviewId
        );

        return await _rdfTripleRepository.CreateUpdateRdfTriple(rdfTripleDto);
    }
}