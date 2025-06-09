using ExtractorSemanticoApi.Application.Dto.Sentiments;

namespace ExtractorSemanticoApi.Application.Interfaces;
public interface ISentimentRepository
{
    Task<SentimentResponseDto> CreateUpdateSentiment(SentimentRequestDto request);
    Task<List<SentimentResponseDto>> GetSentimentsByReviewId(int reviewId);
    Task AnalyzeReviewSentiments(int reviewId);
    Task DeleteSentimentsByReviewId(int reviewId);

}
