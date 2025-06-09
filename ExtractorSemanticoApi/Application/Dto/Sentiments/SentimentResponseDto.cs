namespace ExtractorSemanticoApi.Application.Dto.Sentiments;
public record SentimentResponseDto(
    int SentimentId,
    int ReviewId,
    string Aspect,
    string SentimentType,
    float? Confidence
);
