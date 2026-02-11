using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using EcommerceRazorApp.Services.Interfaces;
using EcommerceRazorApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceRazorApp.Pages.Admin
{
    public class AdminIndexModel : PageModel
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminIndexModel> _logger;

        public AdminIndexModel(IAdminService adminService, ILogger<AdminIndexModel> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        public int ProductCount { get; set; }
        public int CategoryCount { get; set; }
        public List<Product> RecentProducts { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                var products = await _adminService.GetAllProductsAsync();
                var categories = await _adminService.GetAllCategoriesAsync();

                ProductCount = products?.Count ?? 0;
                CategoryCount = categories?.Count ?? 0;

                RecentProducts = products?
                    .OrderByDescending(p => p.ProductId)
                    .Take(4)
                    .ToList() ?? new List<Product>();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to load admin dashboard data");
                TempData["ErrorMessage"] = "Unable to load admin dashboard. Check logs for details.";
            }
        }
    }
}