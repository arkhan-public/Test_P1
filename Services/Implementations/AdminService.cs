using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using EcommerceRazorApp.Models;
using EcommerceRazorApp.Repositories.Interfaces;
using EcommerceRazorApp.Services.Interfaces;

namespace EcommerceRazorApp.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<AdminService> _logger;

        public AdminService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            ILogger<AdminService> logger)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        // Products
        public async Task<List<Product>> GetAllProductsAsync()
        {
            try
            {
                return await _productRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllProductsAsync failed");
                throw;
            }
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            try
            {
                return await _productRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetProductByIdAsync failed for {Id}", id);
                throw;
            }
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            try
            {
                await _productRepository.AddAsync(product);
                _logger.LogInformation("Created product {ProductId}", product.ProductId);
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateProductAsync failed");
                throw;
            }
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            try
            {
                var result = await _productRepository.UpdateAsync(product);
                if (result) _logger.LogInformation("Updated product {ProductId}", product.ProductId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateProductAsync failed for {ProductId}", product.ProductId);
                throw;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var result = await _productRepository.DeleteAsync(id);
                if (result) _logger.LogInformation("Deleted product {ProductId}", id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteProductAsync failed for {ProductId}", id);
                throw;
            }
        }

        // Categories
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            try
            {
                return await _categoryRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllCategoriesAsync failed");
                throw;
            }
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            try
            {
                return await _categoryRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetCategoryByIdAsync failed for {Id}", id);
                throw;
            }
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            try
            {
                await _categoryRepository.AddAsync(category);
                _logger.LogInformation("Created category {CategoryId}", category.CategoryId);
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateCategoryAsync failed");
                throw;
            }
        }

        public async Task<bool> UpdateCategoryAsync(Category category)
        {
            try
            {
                var result = await _categoryRepository.UpdateAsync(category);
                if (result) _logger.LogInformation("Updated category {CategoryId}", category.CategoryId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateCategoryAsync failed for {CategoryId}", category.CategoryId);
                throw;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                var result = await _categoryRepository.DeleteAsync(id);
                if (result) _logger.LogInformation("Deleted category {CategoryId}", id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteCategoryAsync failed for {CategoryId}", id);
                throw;
            }
        }
    }
}