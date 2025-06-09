namespace ExtractorSemanticoApi.Application.Dto.Products;
public record ProductRequestDto(
    int? ProductId,
    string Name,
    string? Brand,
    string? Model,
    string? Category
);