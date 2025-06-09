namespace ExtractorSemanticoApi.Application.Dto.RdfTriples;
public record RdfTripleResponseDto(
    int TripleId,
    string Subject,
    string Predicate,
    string Object,
    int? SourceReviewId
);