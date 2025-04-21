using Application;
using Bogus;
using Domain.Entities;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Respawn;
using System.Reflection;
using Bogus.Extensions;
using Infrastructure.Interfaces;
using MigrationRunner = Infrastructure.DataBase.MigrationRunner;

namespace ApplicationIntegrationTests
{
    public sealed class TestingFixture : IAsyncLifetime
    {
        private readonly Faker _faker;
        private Respawner _respawner = null!;

        public TestingFixture()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) => { config.AddJsonFile("appsettings.json"); })
                .ConfigureServices((context, services) =>
                {
                    services.AddInfrastructure();
                    services.AddApplication();

                    var connectionString = context.Configuration.GetConnectionString("PostgresDB");
                    if (string.IsNullOrWhiteSpace(connectionString))
                        throw new ApplicationException("PostgresDB connection string is empty");

                    services.AddSingleton(_ => new NpgsqlDataSourceBuilder(connectionString).Build());
                    services.AddTransient(sp =>
                    {
                        var dataSource = sp.GetRequiredService<NpgsqlDataSource>();
                        return dataSource.CreateConnection();
                    });

                    services
                        .AddFluentMigratorCore()
                        .ConfigureRunner(rb => rb
                            .AddPostgres()
                            .WithGlobalConnectionString(connectionString)
                            .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
                        .Configure<SelectingProcessorAccessorOptions>(options => { options.ProcessorId = "Postgres"; });
                })
                .Build();

            ServiceProvider = host.Services;
            _faker = new Faker();
        }

        public IServiceProvider ServiceProvider { get; }

        public async Task InitializeAsync()
        {
            using var scope = ServiceProvider.CreateScope();
            var connection = scope.ServiceProvider.GetRequiredService<NpgsqlConnection>();
            await connection.OpenAsync();

            var migrationRunner = scope.ServiceProvider.GetRequiredService<MigrationRunner>();
            migrationRunner.Run();

            _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"],
                TablesToIgnore = ["VersionInfo"]
            });
        }

        public async Task<User> CreateUser()
        {
            using var scope = ServiceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var userId = await userRepository.Create(new User
            {
                FullName = _faker.Name.FullName(),
                Email = _faker.Person.Email,
                PhoneNumber = _faker.Phone.PhoneNumber(),
                Role = "User",
                Passport = "1234-567890",
                DateOfBirth = DateTime.Now.AddDays(-10)
            });

            var user = await userRepository.GetById(userId);
            if (user == null) throw new Exception("Can't create user");
            return user;
        }

        public async Task<Apartment> CreateApartment()
        {
            using var scope = ServiceProvider.CreateScope();
            var apartmentRepository = scope.ServiceProvider.GetRequiredService<IApartmentRepository>();
            var user = await CreateUser();

            var apartmentId = await apartmentRepository.Create(new Apartment
            {
                OwnerId = user.Id,
                Title = _faker.Address.City(),
                Description = _faker.Lorem.Paragraph(),
                Address = _faker.Address.FullAddress(),
                PricePerDay = _faker.Random.Double(50, 500),
                NumOfFloor = _faker.Random.Int(1, 5),
                Square = _faker.Random.Double(30, 200),
                Capacity = _faker.Random.Int(1, 10)
            });

            var apartment = await apartmentRepository.GetById(apartmentId);
            if (apartment == null) throw new Exception("Can't create apartment");
            return apartment;
        }

        public async Task<Deal> CreateDeal()
        {
            using var scope = ServiceProvider.CreateScope();
            var dealRepository = scope.ServiceProvider.GetRequiredService<IDealRepository>();
            var user = await CreateUser();
            var apartment = await CreateApartment();

            var dealId = await dealRepository.Create(new Deal
            {
                UserId = user.Id,
                ApartmentId = apartment.Id,
                CheckInDate = DateTime.Now.AddDays(1),
                CheckOutDate = DateTime.Now.AddDays(5),
                TotalPrice = 1000
            });

            var deal = await dealRepository.GetById(dealId);
            if (deal == null) throw new Exception("Can't create deal");
            return deal;
        }

        public async Task<Review> CreateReview()
        {
            using var scope = ServiceProvider.CreateScope();
            var reviewRepository = scope.ServiceProvider.GetRequiredService<IReviewRepository>();
            var user = await CreateUser();
            var apartment = await CreateApartment();

            var reviewId = await reviewRepository.Create(new Review
            {
                UserId = user.Id,
                ApartmentId = apartment.Id,
                Rating = _faker.Random.Int(1, 5),
                Comment = _faker.Lorem.Sentence()
            });

            var review = await reviewRepository.GetById(reviewId);
            if (review == null) throw new Exception("Can't create review");
            return review;
        }

        public async Task DisposeAsync() => await ResetDatabase();

        private async Task ResetDatabase()
        {
            using var scope = ServiceProvider.CreateScope();
            var connection = scope.ServiceProvider.GetRequiredService<NpgsqlConnection>();
            await connection.OpenAsync();
            await _respawner.ResetAsync(connection);
        }
    }
}