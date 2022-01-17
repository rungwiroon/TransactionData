using Application.Commands.ImportTransactions;
using CsvHelper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Upload is null)
                return new PageResult();

            using var memoryStream = new MemoryStream();
            await Upload.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            var actionResultTask = Path.GetExtension(Upload.FileName) switch
            {
                ".csv" => ImportCsv(memoryStream),
                ".xml" => ImportXml(memoryStream),
                _ => throw new NotSupportedException(),
            };

            return await actionResultTask;

            async Task<IActionResult> ImportCsv(MemoryStream memoryStream)
            {
                IReadOnlyList<TransactionItemDTO> transactions;

                try
                {
                    transactions = Helpers.CsvHelper.Read(memoryStream);
                }
                catch (ReaderException)
                {
                    return StatusCode(422);
                }
                
                var commandCreationResult = ImportCsvFileCommand.Create(transactions);

                return await commandCreationResult.MatchAsync(
                    RightAsync: async req =>
                    {
                        await _mediator.Send(req);
                        return (IActionResult)new RedirectToPageResult("Index");
                    },
                    Left: errors =>
                    {
                        TempData["errorsList"] = JsonSerializer.Serialize(errors);
                        return StatusCode(400);
                    });
            }

            async Task<IActionResult> ImportXml(MemoryStream memoryStream)
            {
                IReadOnlyList<TransactionItemDTO>? transactions;

                try
                {
                    transactions = Helpers.XmlHelper.Read(memoryStream);

                    if (transactions == null)
                        return StatusCode(422);
                }
                catch(InvalidOperationException)
                {
                    return StatusCode(422);
                }

                var request = ImportXmlFileCommand.Create(transactions);
                await _mediator.Send(request);

                return new RedirectToPageResult("Index");
            }
        }
    }
}