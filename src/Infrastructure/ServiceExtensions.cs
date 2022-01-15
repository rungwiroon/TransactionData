using Domain;
using Marten;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            });

            return services;
        }
    }
}
