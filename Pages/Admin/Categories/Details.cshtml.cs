using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommerceRazorApp.Services.Interfaces;
using EcommerceRazorApp.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EcommerceRazorApp.Pages.Admin.Categories
{
    public class AdminCategoriesDetailsModel : PageModel
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminCategoriesDetailsModel> _logger;
        public AdminCategoriesDetailsModel(IAdminService adminService, ILogger<AdminCategoriesDetailsModel> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        public Category Category { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Category = await _adminService.GetCategoryByIdAsync(id);
                if (Category == null) return NotFound();
                return Page();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Failed to load category details (id: {Id})", id);
                TempData["ErrorMessage"] = "Unable to load category details.";
                return RedirectToPage("Index");
            }
        }
    }
}