using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommerceRazorApp.Services.Interfaces;
using EcommerceRazorApp.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EcommerceRazorApp.Pages.Admin.Products
{
    public class AdminProductsDeleteModel : PageModel
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminProductsDeleteModel> _logger;
        public AdminProductsDeleteModel(IAdminService adminService, ILogger<AdminProductsDeleteModel> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        [BindProperty]
        public Product Product { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Product = await _adminService.GetProductByIdAsync(id);
                if (Product == null) return NotFound();
                return Page();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to load product for delete (id: {Id})", id);
                TempData["ErrorMessage"] = "Unable to load product.";
                return RedirectToPage("Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var success = await _adminService.DeleteProductAsync(Product.ProductId);
                if (!success)
                {
                    TempData["ErrorMessage"] = "Product not found or could not be deleted.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Product deleted.";
                }
                return RedirectToPage("Index");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error deleting product {ProductId}", Product?.ProductId);
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the product.";
                return RedirectToPage("Index");
            }
        }
    }
}