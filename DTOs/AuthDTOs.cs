namespace ProductsAPI.DTOs;

// O que o cliente envia para se registrar
public class RegisterDTO
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

// O que o cliente envia para fazer login
public class LoginDTO
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

// O que a API retorna após login bem sucedido
public class TokenResponseDTO
{
    public string Token { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}