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
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(ApplicationDbContext context, ILogger<ProductRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            try
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllAsync failed");
                throw;
            }
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Products
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.ProductId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetByIdAsync failed for {Id}", id);
                throw;
            }
        }

        public async Task AddAsync(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddAsync failed");
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            try
            {
                var existing = await _context.Products.FindAsync(product.ProductId);
                if (existing == null) return false;

                existing.Name = product.Name;
                existing.Description = product.Description;
                existing.Price = product.Price;
                existing.Stock = product.Stock;
                existing.IsFeatured = product.IsFeatured;
                existing.ImageUrl = product.ImageUrl;
                existing.CategoryId = product.CategoryId;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateAsync failed for {ProductId}", product.ProductId);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var existing = await _context.Products.FindAsync(id);
                if (existing == null) return false;
                _context.Products.Remove(existing);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteAsync failed for {ProductId}", id);
                throw;
            }
        }
    }
}