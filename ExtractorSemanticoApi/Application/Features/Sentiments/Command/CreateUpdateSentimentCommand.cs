using ExtractorSemanticoApi.Application.Dto.Sentiments;
using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.Sentiments.Command;

public record CreateUpdateSentimentCommand(
    int? SentimentId,
    int ReviewId,
    string Aspect,
    string SentimentType,
    float? Confidence
) : IRequest<SentimentResponseDto>;

public class CreateUpdateSentimentCommandHandler : IRequestHandler<CreateUpdateSentimentCommand, SentimentResponseDto>
{
    private readonly ISentimentRepository _sentimentRepository;

    public CreateUpdateSentimentCommandHandler(ISentimentRepository sentimentRepository)
    {
        _sentimentRepository = sentimentRepository;
    }

    public async Task<SentimentResponseDto> Handle(
        CreateUpdateSentimentCommand request,
        CancellationToken cancellationToken)
    {
        var sentimentDto = new SentimentRequestDto(
            request.SentimentId,
            request.ReviewId,
            request.Aspect,
            request.SentimentType,
            request.Confidence
        );

        return await _sentimentRepository.CreateUpdateSentiment(sentimentDto);
    }
}