using ExtractorSemanticoApi.Application.Dto.RdfTriples;

namespace ExtractorSemanticoApi.Application.Interfaces;
public interface IRdfTripleRepository
{
    Task<RdfTripleResponseDto> CreateUpdateRdfTriple(RdfTripleRequestDto request);
    Task<List<RdfTripleResponseDto>> GetRdfTriples();
    Task<List<RdfTripleResponseDto>> GetRdfTriplesByReviewId(int reviewId);
    Task GenerateRdfTriplesFromReview(int reviewId);
    Task DeleteTriplesByReviewId(int reviewId);
}
