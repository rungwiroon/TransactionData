using Application.Commands;
using Application.Commands.ImportTransactions;
using CsvHelper;
using MediatR;
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
        private readonly IMediator _mediator;

        [BindProperty]
        [Required(ErrorMessage = "Please select a file.")]
        [MaxFileSize(1 * 1024 * 1024)]
        [AllowedExtensions(new string[] { ".csv", ".xml" })]
        public IFormFile? Upload { get; set; }

        public IndexModel(
            IHostEnvironment environment,
            ILogger<IndexModel> logger,
            IMediator mediator)
        {
            _environment = environment;
            _logger = logger;
            _mediator = mediator;
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
            memoryStream.Position = 0;

            var import = Path.GetExtension(Upload.FileName) switch
            {
                ".csv" => ImportCsv(memoryStream),
                ".xml" => ImportXml(memoryStream),
                _ => throw new NotSupportedException(),
            };

            await import;

            async Task<StatusCodeResult> ImportCsv(MemoryStream memoryStream)
            {
                IReadOnlyList<TransactionItemDTO> transactions;

                try
                {
                    transactions = Helpers.CsvHelper.Read(memoryStream);
                }
                catch (InvalidOperationException)
                {
                    return StatusCode(422);
                }
                
                var request = ImportCsvFileCommand.Create(transactions);
                await _mediator.Send(request);

                return StatusCode(200);
            }

            async Task<StatusCodeResult> ImportXml(MemoryStream memoryStream)
            {
                IReadOnlyList<TransactionItemDTO>? transactions;

                try
                {
                    transactions = Helpers.XmlHelper.Read(memoryStream);

                    if (transactions == null)
                        return StatusCode(422);
                }
                catch(ReaderException)
                {
                    return StatusCode(422);
                }

                var request = ImportXmlFileCommand.Create(transactions);
                await _mediator.Send(request);

                return StatusCode(200);
            }
        }
    }
}