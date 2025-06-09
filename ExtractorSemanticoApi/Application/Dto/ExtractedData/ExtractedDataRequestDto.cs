namespace ExtractorSemanticoApi.Application.Dto.ExtractedData;
public record ExtractedDataRequestDto(
    int? DataId,
    int ReviewId,
    string Type,
    string Value,
    string? Subtype,
    Dictionary<string, object>? Metadata
);
