using ExtractorSemanticoApi.Application.Dto.Products;
using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.Products.Queries;

public record GetProductsQuery : IRequest<List<ProductResponseDto>>;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductResponseDto>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<ProductResponseDto>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        return await _productRepository.GetProducts();
    }
}