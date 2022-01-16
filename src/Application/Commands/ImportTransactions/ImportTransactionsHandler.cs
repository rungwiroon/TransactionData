using Domain;
using Infrastructure.Repositories;
using MediatR;

namespace Application.Commands.ImportTransactions
{
    public class ImportTransactionsHandler :
        IRequestHandler<ImportCsvFileCommand, bool>,
        IRequestHandler<ImportXmlFileCommand, bool>
    {
        private readonly ITransactionRepository repository;

        public ImportTransactionsHandler(
            ITransactionRepository repository)
        {
            this.repository = repository;
        }

        public async Task<bool> Handle(ImportCsvFileCommand message, CancellationToken cancellationToken)
        {
            repository.Store(message.Items.Select(MapToDomain));

            await repository.SaveChangesAsync(cancellationToken);

            return true;

            static Transaction MapToDomain(TransactionItemDTO item)
            {
                var status = item.Status switch
                {
                    "Approved" => TransactionStatus.A,
                    "Failed" => TransactionStatus.R,
                    "Finished" => TransactionStatus.D,
                    _ => throw new InvalidTransactionDataException()
                };

                return new Transaction(
                    item.TransactionID!,
                    new TransactionAmount(item.Amount!.Value),
                    new CurrencyCode(item.CurrencyCode!),
                    item.TransactionDate!.Value,
                    status);
            };
        }

        public async Task<bool> Handle(ImportXmlFileCommand message, CancellationToken cancellationToken)
        {
            repository.Store(message.Items.Select(MapToDomain));

            await repository.SaveChangesAsync(cancellationToken);

            return true;

            static Transaction MapToDomain(TransactionItemDTO item) =>
                new(
                    item.TransactionID!,
                    new TransactionAmount(item.Amount!.Value),
                    new CurrencyCode(item.CurrencyCode!),
                    item.TransactionDate!.Value,
                    item.Status switch
                    {
                        "Approved" => TransactionStatus.A,
                        "Rejected" => TransactionStatus.R,
                        "Done" => TransactionStatus.D,
                        _ => throw new InvalidTransactionDataException()
                    });
        }
    }
}
