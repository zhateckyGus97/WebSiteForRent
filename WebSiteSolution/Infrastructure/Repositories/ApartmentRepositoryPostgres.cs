using Dapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Npgsql;

namespace Infrastructure.Repositories
{
    public class ApartmentRepositoryPostgres : IApartmentRepository
    {
        private readonly NpgsqlConnection _connection;
        public ApartmentRepositoryPostgres(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Create(Apartment apartment)
        {
            await _connection.OpenAsync();

            var apartmentId = await _connection.QuerySingleAsync<int>(
                    @"INSERT INTO Apartment (Title, Description, Address, PricePerDay, NumOfFloor, Square, Capacity, PhotoURLs)
                      VAlUES (@Title, @Description, @Address, @PricePerDay, @NumOfFloor, @Square, @Capacity, @PhotoURLs)
                      RETURNING Id",
                    new { apartment.Title, apartment.Description, apartment.Address, apartment.PricePerDay, 
                            apartment.NumOfFloor, apartment.Square, apartment.Capacity, apartment.PhotoURLs });
            await _connection.CloseAsync();
            return apartmentId;
        }

        public async Task<bool> Delete(int id)
        {
            await _connection.OpenAsync();

            var affectedRows = await _connection.ExecuteAsync(
                    @"DELETE FROM Apartment WHERE id = @Id",
                    new { Id = id });

            await _connection.CloseAsync();

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Apartment>> GetAll()
        {
            await _connection.OpenAsync();

            var apartments = await _connection.QueryAsync<Apartment>(
                    @"SELECT * FROM Apartment");

            await _connection.CloseAsync();

            return apartments;
        }

        public async Task<Apartment?> GetById(int id)
        {
            await _connection.OpenAsync();

            var apartment = await _connection.QueryFirstOrDefaultAsync<Apartment>(
                    @"SELECT * FROM Apartment WHERE id = @Id", new { Id = id });

            await _connection.CloseAsync();

            return apartment;
        }

        public async Task<bool> Update(Apartment apartment)
        {
            await _connection.OpenAsync();

            var affectedRows = await _connection.ExecuteAsync(
                    @"UPDATE Apartment SET Title = @Title, 
                                           Description = @Description, 
                                           Address = @Address, 
                                           PricePerDay = @PricePerDay,     
                                           NumOfFloor = @NumOfFloor, 
                                           Square = @Square, 
                                           Capacity = @Capacity, 
                                           PhotoURLs = @PhotoURLs
                    WHERE id = @Id");

            await _connection.CloseAsync();

            return affectedRows > 0;
        }
    }
}
