using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommerceRazorApp.Services.Interfaces;
using EcommerceRazorApp.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EcommerceRazorApp.Pages.Admin.Categories
{
    public class AdminCategoriesEditModel : PageModel
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminCategoriesEditModel> _logger;
        public AdminCategoriesEditModel(IAdminService adminService, ILogger<AdminCategoriesEditModel> logger)
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
                _logger.LogError(ex, "Failed to load category for edit (id: {Id})", id);
                TempData["ErrorMessage"] = "Unable to load category.";
                return RedirectToPage("Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                var success = await _adminService.UpdateCategoryAsync(Category);
                if (!success)
                {
                    TempData["ErrorMessage"] = "Category not found or could not be updated.";
                    return RedirectToPage("Index");
                }

                TempData["SuccessMessage"] = "Category updated successfully.";
                return RedirectToPage("Index");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error updating category {CategoryId}", Category?.CategoryId);
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while updating the category.");
                return Page();
            }
        }
    }
}