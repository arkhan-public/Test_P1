using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommerceRazorApp.Services.Interfaces;
using EcommerceRazorApp.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;

namespace EcommerceRazorApp.Pages.Admin.Products
{
    public class AdminProductsCreateModel : PageModel
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminProductsCreateModel> _logger;

        public AdminProductsCreateModel(IAdminService adminService, ILogger<AdminProductsCreateModel> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        [BindProperty]
        public Product Product { get; set; } = new();

        public SelectList CategoriesSelectList { get; set; } = default!;

        public async Task OnGetAsync()
        {
            try
            {
                var categories = await _adminService.GetAllCategoriesAsync();
                CategoriesSelectList = new SelectList(categories.OrderBy(c => c.Name), "CategoryId", "Name");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to load categories for product creation");
                TempData["ErrorMessage"] = "Unable to load categories.";
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            try
            {
                await _adminService.CreateProductAsync(Product);
                TempData["SuccessMessage"] = "Product created successfully.";
                return RedirectToPage("Index");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error creating product");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while creating the product.");
                await OnGetAsync();
                return Page();
            }
        }
    }
}