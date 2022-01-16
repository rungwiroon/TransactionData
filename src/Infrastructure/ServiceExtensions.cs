using Infrastructure.Repositories;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Postgresql;

namespace Infrastructure
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            string connectionString,
            bool isDevelopment = false)
        {
            services.AddMarten(options =>
            {
                // Establish the connection string to your Marten database
                options.Connection(connectionString);

                DocumentMapper.Config(options);

                // If we're running in development mode, let Marten just take care
                // of all necessary schema building and patching behind the scenes
                if (isDevelopment)
                {
                    options.AutoCreateSchemaObjects = AutoCreate.All;
                }
            })
            // Chained helper to replace the built in
            // session factory behavior
            .BuildSessionsWith<CustomSessionFactory>();

            services.AddScoped<ITransactionRepository, TransactionRepository>();

            return services;
        }
    }
}
