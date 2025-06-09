namespace ExtractorSemanticoApi.Application.Dto.Sentiments;
public record SentimentRequestDto(
    int? SentimentId,
    int ReviewId,
    string Aspect,
    string SentimentType,
    float? Confidence
);
