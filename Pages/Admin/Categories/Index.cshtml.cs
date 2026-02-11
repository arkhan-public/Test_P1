using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommerceRazorApp.Services.Interfaces;
using EcommerceRazorApp.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EcommerceRazorApp.Pages.Admin.Categories
{
    public class AdminCategoriesIndexModel : PageModel
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminCategoriesIndexModel> _logger;

        public AdminCategoriesIndexModel(IAdminService adminService, ILogger<AdminCategoriesIndexModel> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        public IList<Category> Categories { get; set; } = new List<Category>();

        public async Task OnGetAsync()
        {
            try
            {
                Categories = await _adminService.GetAllCategoriesAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to load categories for admin index");
                TempData["ErrorMessage"] = "Unable to load categories. See logs for details.";
                Categories = new List<Category>();
            }
        }
    }
}