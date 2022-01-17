using Application.Queries;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionQueries transactionQueries;

        public TransactionController(ITransactionQueries transactionQueries)
        {
            this.transactionQueries = transactionQueries;
        }

        [HttpGet]
        public async Task<IActionResult> GetByCurrency(
            [FromQuery]string? currencyCode,
            [FromQuery]DateTime? start, [FromQuery]DateTime? end,
            [FromQuery]string? status)
        {
            TransactionStatus txStatus = default;

            if (status != null && !Enum.TryParse(status, out txStatus))
                return new BadRequestObjectResult(new { error = $"Invalid status : {status}" });

            var queryResult = await transactionQueries.GetTransactionsAsync(
                currencyCode: currencyCode == null ? null : new CurrencyCode(currencyCode),
                startDate: start, endDate: end,
                status: status == null ? null : txStatus);

            return new JsonResult(queryResult);
        }
    }
}
