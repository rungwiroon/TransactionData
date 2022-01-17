using Domain.TransactionDomain.AggregateModel;
using Domain.TransactionDomain.Exceptions;
using Infrastructure.Repositories;
using LanguageExt;
using MediatR;
using static LanguageExt.Prelude;

namespace Application.Commands.ImportTransactions
{
    public class ImportTransactionsHandler :
        IRequestHandler<ImportCsvFileCommand, Either<IEnumerable<string>, int>>,
        IRequestHandler<ImportXmlFileCommand, Either<IEnumerable<string>, int>>
    {
        private readonly ITransactionRepository repository;

        public ImportTransactionsHandler(
            ITransactionRepository repository)
        {
            this.repository = repository;
        }

        private async Task<Either<IEnumerable<string>, int>> HandleCoreAsync(
            Func<TransactionItemDTO, Either<string, Transaction>> mapToDomain,
            IEnumerable<TransactionItemDTO> items,
            CancellationToken cancellationToken)
        {
            var mapTransactionsResult = items.Select(mapToDomain).ToArray();

            if (mapTransactionsResult.Any(tx => tx.IsLeft))
            {
                return Left(items.Select(mapToDomain)
                    .Match(
                        Right: _ => String.Empty,
                        Left: errMsg => errMsg));
            }
            else
            {
                repository.Store(items.Select(mapToDomain)
                    .Match(
                        Right: tx => tx,
                        Left: _ => default));

                await repository.SaveChangesAsync(cancellationToken);

                return Right(items.Count());
            }
        }

        public async Task<Either<IEnumerable<string>, int>> Handle(
            ImportCsvFileCommand message,
            CancellationToken cancellationToken)
        {
            return await HandleCoreAsync(MapToDomain, message.Items, cancellationToken);

            static Either<string, Transaction> MapToDomain(TransactionItemDTO item)
            {
                try
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
                        new TransactionAmount(item.Amount!.Value, new CurrencyCode(item.CurrencyCode!)),
                        item.TransactionDate!.Value,
                        status);
                }
                catch(TransactionDomainException ex)
                {
                    return $"Transaction ID : {item.TransactionID}, {ex.Message}";
                }
            };
        }

        public async Task<Either<IEnumerable<string>, int>> Handle(
            ImportXmlFileCommand message,
            CancellationToken cancellationToken)
        {
            return await HandleCoreAsync(MapToDomain, message.Items, cancellationToken);

            static Either<string, Transaction> MapToDomain(TransactionItemDTO item)
            {
                try
                {
                    return new Transaction(
                        item.TransactionID!,
                        new TransactionAmount(item.Amount!.Value, new CurrencyCode(item.CurrencyCode!)),
                        item.TransactionDate!.Value,
                        item.Status switch
                        {
                            "Approved" => TransactionStatus.A,
                            "Rejected" => TransactionStatus.R,
                            "Done" => TransactionStatus.D,
                            _ => throw new InvalidTransactionDataException()
                        });
                }
                catch (TransactionDomainException ex)
                {
                    return $"Transaction ID : {item.TransactionID}, {ex.Message}"; ;
                }
            }
                
        }
    }
}
