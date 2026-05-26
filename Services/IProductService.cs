using ProductsAPI.DTOs;

namespace ProductsAPI.Services;

public interface IProductService
{
    Task<IEnumerable<ProductResponseDTO>> GetAllAsync();
    Task<ProductResponseDTO?> GetByIdAsync(int id);
    Task<ProductResponseDTO> CreateAsync(ProductCreateDTO dto);
    Task<ProductResponseDTO?> UpdateAsync(int id, ProductUpdateDTO dto);
    Task<bool> DeleteAsync(int id);
}