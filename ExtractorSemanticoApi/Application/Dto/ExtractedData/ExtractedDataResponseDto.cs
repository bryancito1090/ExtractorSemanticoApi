namespace ExtractorSemanticoApi.Application.Dto.ExtractedData;
public record ExtractedDataResponseDto(
    int DataId,
    int ReviewId,
    string Type,
    string Value,
    string? Subtype,
    Dictionary<string, object>? Metadata
);