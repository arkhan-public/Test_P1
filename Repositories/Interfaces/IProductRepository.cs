using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceRazorApp.Models;

namespace EcommerceRazorApp.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task<bool> UpdateAsync(Product product);
        Task<bool> DeleteAsync(int id);
    }
}