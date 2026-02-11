using System.Collections.Generic;
using System.Threading.Tasks;
using EcommerceRazorApp.Models;

namespace EcommerceRazorApp.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task<bool> UpdateAsync(Category category);
        Task<bool> DeleteAsync(int id);
    }
}