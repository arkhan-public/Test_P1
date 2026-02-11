using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommerceRazorApp.Services.Interfaces;
using EcommerceRazorApp.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EcommerceRazorApp.Pages.Admin.Categories
{
    public class AdminCategoriesDeleteModel : PageModel
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminCategoriesDeleteModel> _logger;
        public AdminCategoriesDeleteModel(IAdminService adminService, ILogger<AdminCategoriesDeleteModel> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        [BindProperty]
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
                _logger.LogError(ex, "Failed to load category for delete (id: {Id})", id);
                TempData["ErrorMessage"] = "Unable to load category.";
                return RedirectToPage("Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var success = await _adminService.DeleteCategoryAsync(Category.CategoryId);
                if (!success)
                {
                    TempData["ErrorMessage"] = "Category not found or could not be deleted.";
                }
                else
                {
                    TempData["SuccessMessage"] = "Category deleted.";
                }
                return RedirectToPage("Index");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error deleting category {CategoryId}", Category?.CategoryId);
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the category.";
                return RedirectToPage("Index");
            }
        }
    }
}