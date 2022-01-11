using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WebApp.Pages.Validators;

namespace TransactionData.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHostEnvironment _environment;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        [Required(ErrorMessage = "Please select a file.")]
        [MaxFileSize(1 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".csv", ".xml" })]
        public IFormFile Upload { get; set; }

        public IndexModel(
            IHostEnvironment environment,
            ILogger<IndexModel> logger)
        {
            _environment = environment;
            _logger = logger;
        }

        public void OnGet()
        {

        }
        
        public async Task OnPostAsync()
        {
            if(ModelState.IsValid)
            {
                var file = Path.Combine(_environment.ContentRootPath, "uploads", Upload.FileName);
                using (var fileStream = new FileStream(file, FileMode.Create))
                {
                    await Upload.CopyToAsync(fileStream);
                }
            }
        }
    }
}