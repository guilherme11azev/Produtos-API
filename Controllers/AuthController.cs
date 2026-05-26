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
            return Conflict(new { message = "Email já cadastrado." });

        return CreatedAtAction(nameof(Register), result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (result is null)
            return Unauthorized(new { message = "Email ou senha inválidos." });

        return Ok(result);
    }
}