using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductsAPI.DTOs;
using ProductsAPI.Services;

namespace ProductsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Todas as rotas exigem autenticação
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _service.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _service.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")] // Somente Admin
    public async Task<IActionResult> Create([FromBody] ProductCreateDTO dto)
    {
        try
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")] // Somente Admin
    public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDTO dto)
    {
        try
        {
            var updated = await _service.UpdateAsync(id, dto);
            return updated is null ? NotFound() : Ok(updated);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] // Somente Admin
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}