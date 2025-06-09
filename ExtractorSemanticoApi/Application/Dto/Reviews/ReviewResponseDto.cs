namespace ExtractorSemanticoApi.Application.Dto.Reviews;
// Update the ReviewResponseDto to use a mutable property for CleanText
public record ReviewResponseDto(
    int ReviewId,
    int ProductId,
    string UserName,
    string OriginalText,
    string? CleanText,
    int? Rating,
    DateTime? ReviewDate,
    string? Location
);