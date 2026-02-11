using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using EcommerceRazorApp.Data;
using EcommerceRazorApp.Models;
using EcommerceRazorApp.Repositories.Interfaces;

namespace EcommerceRazorApp.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(ApplicationDbContext context, ILogger<CategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            try
            {
                return await _context.Categories.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllAsync failed");
                throw;
            }
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetByIdAsync failed for {Id}", id);
                throw;
            }
        }

        public async Task AddAsync(Category category)
        {
            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddAsync failed");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            try
            {
                var existing = await _context.Categories.FindAsync(category.CategoryId);
                if (existing == null) return false;

                existing.Name = category.Name;
                existing.Description = category.Description;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateAsync failed for {CategoryId}", category.CategoryId);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var existing = await _context.Categories.FindAsync(id);
                if (existing == null) return false;
                _context.Categories.Remove(existing);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteAsync failed for {CategoryId}", id);
                throw;
            }
        }
    }
}