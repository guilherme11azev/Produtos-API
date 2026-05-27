using Microsoft.AspNetCore.Mvc;
using ProductsAPI.DTOs;
using ProductsAPI.Services;

namespace ProductsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        var result = await _authService.RegisterAsync(dto);

        if (result is null)
            return Conflict(new { message = "Email ja cadastrado." });

        return CreatedAtAction(nameof(Register), result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (result is null)
            return Unauthorized(new { message = "Email ou senha invalidos." });

        return Ok(result);
    }

    [HttpPost("make-admin/{userId}")]
    public async Task<IActionResult> MakeAdmin(int userId)
    {
        var result = await _authService.MakeAdminAsync(userId);

        if (!result)
            return NotFound(new { message = "Usuario nao encontrado." });

        return Ok(new { message = "Usuario promovido a Admin com sucesso." });
    }
}