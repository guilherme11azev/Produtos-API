using ProductsAPI.Models;

namespace ProductsAPI.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<User> CreateAsync(User user);
}