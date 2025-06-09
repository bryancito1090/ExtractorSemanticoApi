using ExtractorSemanticoApi.Application.Dto.Products;
using ExtractorSemanticoApi.Application.Interfaces;
using MediatR;

namespace ExtractorSemanticoApi.Application.Features.Products.Commands;
public record CreateUpdateProductCommand(
    int? ProductId,
    string Name,
    string? Brand,
    string? Model,
    string? Category
) : IRequest<ProductResponseDto>;

public class CreateUpdateProductCommandHandler : IRequestHandler<CreateUpdateProductCommand, ProductResponseDto>
{
    private readonly IProductRepository _productRepository;

    public CreateUpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductResponseDto> Handle(
        CreateUpdateProductCommand request,
        CancellationToken cancellationToken)
    {
        var productDto = new ProductRequestDto(
            request.ProductId,
            request.Name,
            request.Brand,
            request.Model,
            request.Category
        );

        return await _productRepository.CreateUpdateProduct(productDto);
    }
}
