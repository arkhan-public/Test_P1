using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using EcommerceRazorApp.Services.Interfaces;
using EcommerceRazorApp.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EcommerceRazorApp.Pages.Admin.Categories
{
    public class AdminCategoriesCreateModel : PageModel
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<AdminCategoriesCreateModel> _logger;
        public AdminCategoriesCreateModel(IAdminService adminService, ILogger<AdminCategoriesCreateModel> logger) {
            _adminService = adminService;
            _logger = logger;
        }

        [BindProperty]
        public Category Category { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                await _adminService.CreateCategoryAsync(Category);
                TempData["SuccessMessage"] = "Category created successfully.";
                return RedirectToPage("Index");
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error creating category");
                ModelState.AddModelError(string.Empty, "An unexpected error occurred while creating the category.");
                return Page();
            }
        }
    }
}