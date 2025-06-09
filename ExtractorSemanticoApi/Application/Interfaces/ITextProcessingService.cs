namespace ExtractorSemanticoApi.Application.Interfaces;
public interface ITextProcessingService
{
    Task ProcessReviewText(int reviewId);
    Task AnalyzeReviewSentiments(int reviewId);
    Task GenerateRdfTriplesFromReview(int reviewId);
}