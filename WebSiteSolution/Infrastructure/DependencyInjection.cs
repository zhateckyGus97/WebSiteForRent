using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Npgsql;
using FluentMigrator.Runner;
using System.Reflection;
namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionstring = configuration.GetConnectionString("PostgreDB");
                return new NpgsqlDataSourceBuilder(connectionstring).Build();
            });

            services.AddScoped(sp =>
            {
                var datasource = sp.GetRequiredService<NpgsqlDataSource>();
                return datasource.CreateConnection();
            });

            services.AddTransient<IUserRepository, PostgresUserRepository>(); //еще 3 таких

            /*services.AddSingleton<IUserRepository, InMemoryUserRepository>();*/
            services.AddSingleton<IDealRepository, InMemoryDealRepository>();
            services.AddSingleton<IReviewRepository, InMemoryReviewRepository>();
            services.AddSingleton<IApartmentRepository, InMemoryApartmentRepository>();

            services.AddFluentMigratorCore()
                .ConfigureRunner(
                    rb => rb.AddPostgres()
                    .WithGlobalConnectionString("PostgresDB")
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations()
                    )
                .AddLogging(lb => lb.AddFluentMigratorConsole());
            services.AddScoped<DataBase.MigrationRunner>();

            return services;
        }
    }
}
