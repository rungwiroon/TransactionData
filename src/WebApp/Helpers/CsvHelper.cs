using Application.Commands;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace WebApp.Helpers
{
    public class CsvHelper
    {
        public class TransactionItemMap : ClassMap<TransactionItemDTO>
        {
            public TransactionItemMap()
            {
                Map(m => m.TransactionID).Index(0);
                Map(m => m.Amount).Index(1).TypeConverterOption.NumberStyles(NumberStyles.Number);
                Map(m => m.CurrencyCode).Index(2);
                Map(m => m.TransactionDate).Index(3)
                    .TypeConverterOption.DateTimeStyles(
                        DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite)
                    .TypeConverterOption.Format("dd/MM/yyyy HH:mm:ss");
                Map(m => m.Status).Index(4);
            }
        }

        public static IReadOnlyList<TransactionItemDTO> Read(Stream stream)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
                BadDataFound = x =>
                {
                    Console.WriteLine($"Bad data: <{x.RawRecord}>");
                },
                TrimOptions = TrimOptions.Trim,
            };

            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, config);
            csv.Context.RegisterClassMap<TransactionItemMap>();
            var records = csv.GetRecords<TransactionItemDTO>();

            return records.ToList().AsReadOnly();
        }
    }
}
