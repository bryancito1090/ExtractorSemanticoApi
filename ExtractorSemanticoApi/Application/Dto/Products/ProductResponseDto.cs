namespace ExtractorSemanticoApi.Application.Dto.Products;
public record ProductResponseDto(
    int ProductId,
    string Name,
    string? Brand,
    string? Model,
    string? Category
);