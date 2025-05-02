using FluentMigrator.Runner;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Repositories.PostgresRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
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
                var connectionstring = configuration.GetConnectionString("PostgresDB");
                return new NpgsqlDataSourceBuilder(connectionstring).Build();
            });

            services.AddScoped(sp =>
            {
                var datasource = sp.GetRequiredService<NpgsqlDataSource>();
                return datasource.CreateConnection();
            });

            services.AddTransient<IUserRepository, UserPostgresRepository>();
            services.AddTransient<IDealRepository, DealPostgresRepository>();
            services.AddTransient<IApartmentRepository, ApartmentPostgresRepository>();
            services.AddTransient<IReviewRepository, ReviewPostgresRepository>();
            services.AddTransient<IAttachmentRepository, AttachmentPostgresRepository>();

            DapperConfig.Configure();
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
