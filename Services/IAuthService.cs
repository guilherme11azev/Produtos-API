using ProductsAPI.DTOs;

namespace ProductsAPI.Services;

public interface IAuthService
{
    Task<TokenResponseDTO?> RegisterAsync(RegisterDTO dto);
    Task<TokenResponseDTO?> LoginAsync(LoginDTO dto);
}