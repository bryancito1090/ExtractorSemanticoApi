namespace ExtractorSemanticoApi.Application.Dto.RdfTriples;
public record RdfTripleRequestDto(
    int? TripleId,
    string Subject,
    string Predicate,
    string Object,
    int? SourceReviewId
);
