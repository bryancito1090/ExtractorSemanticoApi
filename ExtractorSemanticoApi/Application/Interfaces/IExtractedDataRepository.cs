using ExtractorSemanticoApi.Application.Dto.ExtractedData;

namespace ExtractorSemanticoApi.Application.Interfaces;
public interface IExtractedDataRepository
{
    Task<ExtractedDataResponseDto> CreateUpdateExtractedData(ExtractedDataRequestDto request);
    Task<List<ExtractedDataResponseDto>> GetExtractedDataByReviewId(int reviewId);
}

