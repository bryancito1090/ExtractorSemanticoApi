using ExtractorSemanticoApi.Application.Dto.Products;
using ExtractorSemanticoApi.Application.Interfaces;
using ExtractorSemanticoApi.Dominio;
using Microsoft.EntityFrameworkCore;
using System;

namespace ExtractorSemanticoApi.Persistencia.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ExtractorsemanticoContext _context;

    public ProductRepository(ExtractorsemanticoContext context)
    {
        _context = context;
    }

    public async Task<ProductResponseDto> CreateUpdateProduct(ProductRequestDto request)
    {
        Product entity;

        if (request.ProductId.HasValue && request.ProductId > 0)
        {
            // Update existing product
            entity = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductId == request.ProductId)
                ?? throw new KeyNotFoundException($"Product with ID {request.ProductId} not found");

            entity.Name = request.Name;
            entity.Brand = request.Brand;
            entity.Model = request.Model;
            entity.Category = request.Category;

            _context.Products.Update(entity);
        }
        else
        {
            // Create new product
            entity = new Product
            {
                Name = request.Name,
                Brand = request.Brand,
                Model = request.Model,
                Category = request.Category
            };

            await _context.Products.AddAsync(entity);
        }

        await _context.SaveChangesAsync();

        return new ProductResponseDto(
            entity.ProductId,
            entity.Name,
            entity.Brand,
            entity.Model,
            entity.Category
        );
    }

    public async Task<List<ProductResponseDto>> GetProducts()
    {
        return await _context.Products
            .Select(p => new ProductResponseDto(
                p.ProductId,
                p.Name,
                p.Brand,
                p.Model,
                p.Category))
            .ToListAsync();
    }

    public async Task<ProductResponseDto> GetProductById(int id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.ProductId == id)
            ?? throw new KeyNotFoundException($"Product with ID {id} not found");

        return new ProductResponseDto(
            product.ProductId,
            product.Name,
            product.Brand,
            product.Model,
            product.Category);
    }
}