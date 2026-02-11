using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using EcommerceRazorApp.Services.Interfaces;
using EcommerceRazorApp.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;

namespace EcommerceRazorApp.Pages.Admin.Products
{
    public class AdminProductsEditModel : PageModel
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminProductsEditModel> _logger;
        public AdminProductsEditModel(IAdminService adminService, ILogger<AdminProductsEditModel> logger) => (_adminService, _logger) = (adminService, logger);

        [BindProperty]
        public Product Product { get; set; } = new();

        public SelectList CategoriesSelectList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Product = await _adminService.GetProductByIdAsync(id);
                if (Product == null) return NotFound();

                var categories = await _adminService.GetAllCategoriesAsync();
                CategoriesSelectList = new SelectList(categories.OrderBy(c => c.Name), "CategoryId", "Name", Product.CategoryId);

                return Page();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to load product for edit (id: {Id})", id);
                TempData["ErrorMessage"] = "Unable to load product for edit.";
                return RedirectToPage("Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Helpful debug: log ModelState errors so you can see exact binder messages
            if (!ModelState.IsValid)
            {
                foreach (var kv in ModelState)
                {
                    var key = kv.Key;
                    var errors = string.Join("; ", kv.Value.Errors.Select(e => (e.ErrorMessage ?? "") + (e.Exception?.Message != null ? " (" + e.Exception.Message + ")" : "")));
                    _logger.LogWarning("ModelState[{Key}] errors: {Errors}", key, errors);
                }

                var categories = await _adminService.GetAllCategoriesAsync();
                CategoriesSelectList = new SelectList(categories.OrderBy(c => c.Name), "CategoryId", "Name", Product?.CategoryId);
                return Page();
            }

            // Server-side validation: placeholder sentinel value (0) is not a valid selection
            if (Product.CategoryId == 0)
            {
                ModelState.AddModelError("Product.CategoryId", "Please select a category.");
                var categories = await _adminService.GetAllCategoriesAsync();
                CategoriesSelectList = new SelectList(categories.OrderBy(c => c.Name), "CategoryId", "Name", Product?.CategoryId);
                return Page();
            }

            try
            {
                var updated = await _adminService.UpdateProductAsync(Product);
                if (!updated)
                {
                    TempData["ErrorMessage"] = "Product not found or could not be updated.";
                    return RedirectToPage("Index");
                }

                TempData["SuccessMessage"] = "Product updated successfully.";
                return RedirectToPage("Index");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error updating product {ProductId}", Product?.ProductId);
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while updating the product.");
                var categories = await _adminService.GetAllCategoriesAsync();
                CategoriesSelectList = new SelectList(categories.OrderBy(c => c.Name), "CategoryId", "Name", Product?.CategoryId);
                return Page();
            }
        }
    }
}