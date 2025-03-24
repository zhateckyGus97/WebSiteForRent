using Dapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Npgsql;

namespace Infrastructure.Repositories.PostgresRepositories
{
    public class ApartmentPostgresRepository : IApartmentRepository
    {
        private readonly NpgsqlConnection _connection;
        public ApartmentPostgresRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Create(Apartment apartment)
        {
            var apartmentId = await _connection.QuerySingleAsync<int>(
                    @"INSERT INTO Apartments (Title, Description, Address, PricePerDay, NumOfFloor, Square, Capacity)
                      VAlUES (@Title, @Description, @Address, @PricePerDay, @NumOfFloor, @Square, @Capacity)
                      RETURNING Id",
                    new
                    {
                        apartment.Title,
                        apartment.Description,
                        apartment.Address,
                        apartment.PricePerDay,
                        apartment.NumOfFloor,
                        apartment.Square,
                        apartment.Capacity
                    });
            return apartmentId;
        }

        public async Task<bool> Delete(int id)
        {
            var affectedRows = await _connection.ExecuteAsync(
                    @"DELETE FROM Apartments WHERE id = @Id",
                    new { Id = id });

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Apartment>> GetAll()
        {
            var apartments = await _connection.QueryAsync<Apartment>(
                    @"SELECT Id, Title, Description, Address, PricePerDay, NumOfFloor, Square, Capacity FROM Apartments");

            return apartments;
        }

        public async Task<Apartment?> GetById(int id)
        {
            var apartment = await _connection.QueryFirstOrDefaultAsync<Apartment>(
                    @"SELECT Id, Title, Description, Address, PricePerDay, NumOfFloor, Square, Capacity FROM Apartments WHERE id = @Id", new { Id = id });

            return apartment;
        }

        public async Task<bool> Update(Apartment apartment)
        {
            var affectedRows = await _connection.ExecuteAsync(
                    @"UPDATE Apartments SET Title = @Title, 
                                           Description = @Description, 
                                           Address = @Address, 
                                           PricePerDay = @PricePerDay,     
                                           NumOfFloor = @NumOfFloor, 
                                           Square = @Square, 
                                           Capacity = @Capacity 
                    WHERE id = @Id");

            return affectedRows > 0;
        }
    }
}
