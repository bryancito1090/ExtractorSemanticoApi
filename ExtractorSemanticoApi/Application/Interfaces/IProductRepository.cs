using ExtractorSemanticoApi.Application.Dto.Products;

namespace ExtractorSemanticoApi.Application.Interfaces;

public interface IProductRepository
{
    Task<ProductResponseDto> CreateUpdateProduct(ProductRequestDto request);
    Task<List<ProductResponseDto>> GetProducts();
    Task<ProductResponseDto> GetProductById(int id);
}