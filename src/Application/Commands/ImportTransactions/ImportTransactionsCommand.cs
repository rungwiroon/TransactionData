using FluentValidation;
using LanguageExt;
using MediatR;
using static LanguageExt.Prelude;

namespace Application.Commands.ImportTransactions
{
    public class TransactionItemDTO
    {
        public string? TransactionID { get; set; }

        public decimal? Amount { get; set; }

        public string? CurrencyCode { get; set; }

        public DateTime? TransactionDate { get; set; }

        public String? Status { get; set; }
    }

    public abstract class FileValidator : AbstractValidator<TransactionItemDTO>
    {
        public abstract string[] StatusList { get; }

        public FileValidator()
        {
            RuleFor(tx => tx.TransactionID)
                .NotNull()
                .NotEmpty()
                .Length(5, 50);

            RuleFor(tx => tx.Amount)
                .NotNull();

            RuleFor(tx => tx.CurrencyCode)
                .NotNull()
                .NotEmpty()
                .Length(3);

            RuleFor(tx => tx.Status)
                .NotNull()
                .NotEmpty()
                .Must(s => StatusList.Contains(s));
        }
    }

    public class CsvFileValidator : FileValidator
    {
        public override string[] StatusList => new string[]
        {
            "Approved",
            "Failed",
            "Finished",
        };

        public CsvFileValidator()
            : base()
        {
        }
    }

    public class XmlFileValidator : FileValidator
    {
        public override string[] StatusList => new string[]
        {
            "Approved",
            "Rejected",
            "Done",
        };

        public XmlFileValidator()
            : base()
        {
        }
    }

    public abstract class ImportFileCommand
    {
        public IEnumerable<TransactionItemDTO> Items { get; protected set; }

        protected static readonly
            Func<FileValidator, Func<IEnumerable<TransactionItemDTO>, IEnumerable<string>>> 
            Validate =
            validator =>
            items =>
            items.SelectMany((item, idx) =>
            {
                var results = validator.Validate(item);

                return !results.IsValid
                  ? results.Errors
                        .Select(failure => $"Row : {idx}, Column : {failure.PropertyName} failed validation. Error was: {failure.ErrorMessage}")
                  : Enumerable.Empty<string>();
            });
    }

    public class ImportCsvFileCommand 
        : ImportFileCommand, ICommand, IRequest<Either<IEnumerable<string>, int>>
    {
        private ImportCsvFileCommand(IEnumerable<TransactionItemDTO> items)
        {
            Items = items;
        }

        public static Either<IEnumerable<string>, ImportCsvFileCommand> Create(
            IEnumerable<TransactionItemDTO> items)
        {
            var validateCsvFile = Validate(new CsvFileValidator());
            var validationFailedResults = validateCsvFile(items);

            return validationFailedResults.Any()
                ? Left(validationFailedResults)
                : Right(new ImportCsvFileCommand(items));
        }
    }

    public class ImportXmlFileCommand
        : ImportFileCommand, ICommand, IRequest<Either<IEnumerable<string>, int>>
    {
        private ImportXmlFileCommand(IEnumerable<TransactionItemDTO> items)
        {
            Items = items;
        }

        public static Either<IEnumerable<string>, ImportXmlFileCommand> Create(
            IEnumerable<TransactionItemDTO> items)
        {
            var validateCsvFile = Validate(new XmlFileValidator());
            var validationFailedResults = validateCsvFile(items);

            return validationFailedResults.Any()
                ? Left(validationFailedResults)
                : Right(new ImportXmlFileCommand(items));
        }
    }
}
