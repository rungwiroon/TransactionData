using Application.Commands;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using WebApp.Pages.Validators;

namespace WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHostEnvironment _environment;
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        [Required(ErrorMessage = "Please select a file.")]
        [MaxFileSize(1 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".csv", ".xml" })]
        public IFormFile? Upload { get; set; }

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
            if (!ModelState.IsValid || Upload is null)
                return;

            using var memoryStream = new MemoryStream();
            await Upload.CopyToAsync(memoryStream);

            var records = Path.GetExtension(Upload.FileName) switch
            {
                ".csv" => Helpers.CsvHelper.Read(memoryStream),
                ".xml" => Enumerable.Empty<TransactionItem>(),
                _ => throw new NotSupportedException(),
            };
        }
    }
}