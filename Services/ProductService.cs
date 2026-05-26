using ProductsAPI.DTOs;
using ProductsAPI.Models;
using ProductsAPI.Repositories;

namespace ProductsAPI.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProductResponseDTO>> GetAllAsync()
    {
        var products = await _repository.GetAllAsync();
        return products.Select(ToResponseDTO);
    }

    public async Task<ProductResponseDTO?> GetByIdAsync(int id)
    {
        var product = await _repository.GetByIdAsync(id);
        return product is null ? null : ToResponseDTO(product);
    }

    public async Task<ProductResponseDTO> CreateAsync(ProductCreateDTO dto)
    {
        if (dto.Price <= 0)
            throw new ArgumentException("O preço deve ser maior que zero.");

        if (dto.Stock < 0)
            throw new ArgumentException("O estoque não pode ser negativo.");

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock
        };

        var created = await _repository.CreateAsync(product);
        return ToResponseDTO(created);
    }

    public async Task<ProductResponseDTO?> UpdateAsync(int id, ProductUpdateDTO dto)
    {
        if (dto.Price <= 0)
            throw new ArgumentException("O preço deve ser maior que zero.");

        if (dto.Stock < 0)
            throw new ArgumentException("O estoque não pode ser negativo.");

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock
        };

        var updated = await _repository.UpdateAsync(id, product);
        return updated is null ? null : ToResponseDTO(updated);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static ProductResponseDTO ToResponseDTO(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Price = p.Price,
        Stock = p.Stock,
        CreatedAt = p.CreatedAt
    };
}