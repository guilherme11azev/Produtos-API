using Microsoft.EntityFrameworkCore;
using ProductsAPI.Data;
using ProductsAPI.Models;

namespace ProductsAPI.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> CreateAsync(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> UpdateAsync(int id, Product product)
    {
        var existing = await _context.Products.FindAsync(id);
        if (existing is null) return null;

        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Price = product.Price;
        existing.Stock = product.Stock;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product is null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }
}