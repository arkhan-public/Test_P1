using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommerceRazorApp.Services.Interfaces;
using EcommerceRazorApp.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EcommerceRazorApp.Pages.Admin.Products
{
    public class AdminProductsDetailsModel : PageModel
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminProductsDetailsModel> _logger;
        public AdminProductsDetailsModel(IAdminService adminService, ILogger<AdminProductsDetailsModel> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

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
                _logger.LogError(ex, "Failed to load product details (id: {Id})", id);
                TempData["ErrorMessage"] = "Unable to load product details.";
                return RedirectToPage("Index");
            }
        }
    }
}