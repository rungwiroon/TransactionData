using Application.Commands;
using Application.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddTransactionDataApplication(
            this IServiceCollection services)
        {
            services.AddScoped<ITransactionQueries, TransactionQueries>();
            services.AddMediatR(typeof(ICommand));

            return services;
        }
    }
}
