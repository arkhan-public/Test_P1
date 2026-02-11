using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommerceRazorApp.Services.Interfaces;
using EcommerceRazorApp.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceRazorApp.Pages.Admin.Products
{
    public class AdminProductsIndexModel : PageModel
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminProductsIndexModel> _logger;

        public AdminProductsIndexModel(IAdminService adminService, ILogger<AdminProductsIndexModel> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        public IList<Product> Products { get; set; } = new List<Product>();

        public async Task OnGetAsync()
        {
            try
            {
                Products = await _adminService.GetAllProductsAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to load products for admin index");
                TempData["ErrorMessage"] = "Unable to load products. See logs for details.";
                Products = new List<Product>();
            }
        }
    }
}