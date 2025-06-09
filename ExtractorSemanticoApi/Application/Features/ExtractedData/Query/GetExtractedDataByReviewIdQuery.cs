using ExtractorSemanticoApi.Application.Dto.ExtractedData;
using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.ExtractedData.Query;

public record GetExtractedDataByReviewIdQuery(int ReviewId)
    : IRequest<List<ExtractedDataResponseDto>>;

public class GetExtractedDataByReviewIdQueryHandler
    : IRequestHandler<GetExtractedDataByReviewIdQuery, List<ExtractedDataResponseDto>>
{
    private readonly IExtractedDataRepository _extractedDataRepository;

    public GetExtractedDataByReviewIdQueryHandler(IExtractedDataRepository extractedDataRepository)
    {
        _extractedDataRepository = extractedDataRepository;
    }

    public async Task<List<ExtractedDataResponseDto>> Handle(
        GetExtractedDataByReviewIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _extractedDataRepository.GetExtractedDataByReviewId(request.ReviewId);
    }
}
