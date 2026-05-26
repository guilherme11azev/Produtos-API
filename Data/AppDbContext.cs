using Microsoft.EntityFrameworkCore;
using ProductsAPI.Models;

namespace ProductsAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Email deve ser único — não pode ter dois usuários com o mesmo email
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Precisão do decimal para preços
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        // Seed: produto inicial para não começar vazio
        modelBuilder.Entity<Product>().HasData(
            new Product
            {
                Id = 1,
                Name = "Produto Exemplo",
                Description = "Produto criado automaticamente",
                Price = 99.90m,
                Stock = 100,
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}