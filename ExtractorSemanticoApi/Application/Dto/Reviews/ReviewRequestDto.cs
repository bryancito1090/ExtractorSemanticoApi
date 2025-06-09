namespace ExtractorSemanticoApi.Application.Dto.Reviews;
public record ReviewRequestDto(
    int? ReviewId,
    int ProductId,
    string UserName,
    string OriginalText,
    string? CleanText,
    int? Rating,
    DateTime? ReviewDate,
    string? Location
);