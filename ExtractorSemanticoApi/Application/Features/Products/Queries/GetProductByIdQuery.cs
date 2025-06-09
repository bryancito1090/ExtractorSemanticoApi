using ExtractorSemanticoApi.Application.Dto.Products;
using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.Products.Queries;
public record GetProductByIdQuery(int Id) : IRequest<ProductResponseDto>;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponseDto>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponseDto> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        return await _productRepository.GetProductById(request.Id);
    }
}