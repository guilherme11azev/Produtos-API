using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ProductsAPI.DTOs;
using ProductsAPI.Models;
using ProductsAPI.Repositories;

namespace ProductsAPI.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<TokenResponseDTO?> RegisterAsync(RegisterDTO dto)
    {
        // Regra de negócio: email já cadastrado
        var emailExists = await _userRepository.EmailExistsAsync(dto.Email);
        if (emailExists)
            return null;

        // Gera o hash da senha — nunca salva em texto puro
        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.User
        };

        var created = await _userRepository.CreateAsync(user);
        return GenerateToken(created);
    }

    public async Task<TokenResponseDTO?> LoginAsync(LoginDTO dto)
    {
        // Busca o usuário pelo email
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user is null)
            return null;

        // Verifica se a senha confere com o hash salvo no banco
        var passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!passwordValid)
            return null;

        return GenerateToken(user);
    }

    private TokenResponseDTO GenerateToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"]!;
        var expirationHours = int.Parse(jwtSettings["ExpirationHours"]!);
        var expiresAt = DateTime.UtcNow.AddHours(expirationHours);

        // Claims são as informações que ficam dentro do token
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        // Assina o token com a chave secreta
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // Monta o token JWT
        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        return new TokenResponseDTO
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Name = user.Name,
            Email = user.Email,
            Role = user.Role.ToString(),
            ExpiresAt = expiresAt
        };
    }
}