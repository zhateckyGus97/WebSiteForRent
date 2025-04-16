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
                    @"INSERT INTO apartments (owner_id, title, description, address, price_per_day, num_of_floor, square, capacity)
                      VAlUES (@OwnerId, @Title, @Description, @Address, @PricePerDay, @NumOfFloor, @Square, @Capacity)
                      RETURNING Id",
                    apartment
                    );

            return apartmentId;
        }

        public async Task<bool> Delete(int id)
        {
            var affectedRows = await _connection.ExecuteAsync(
                    @"DELETE 
                      FROM apartments 
                      WHERE id = @Id",
                    new { Id = id });

            return affectedRows > 0;
        }

        public async Task DeleteByOwnerId(int ownerid)
        {
            await _connection.ExecuteAsync("DELETE FROM apartments WHERE owner_id = @OwnerId", new { OwnerId = ownerid });
        }

        public async Task<IEnumerable<Apartment>> GetAll()
        {
            var apartments = await _connection.QueryAsync<Apartment>(
                    @"SELECT 
                        id,
                        owner_id,
                        title,  
                        description,  
                        address,  
                        price_per_day,  
                        num_of_floor,  
                        square,  
                        capacity  
                      FROM apartments");

            return apartments;
        }

        public async Task<Apartment?> GetById(int id)
        {
            var apartment = await _connection.QueryFirstOrDefaultAsync<Apartment>(
                    @"SELECT 
                        id,  
                        owner_id,
                        title,  
                        description,  
                        address,  
                        price_per_day,  
                        num_of_floor,  
                        square,  
                        capacity  
                      FROM apartments
                      WHERE id = @Id",
                    new
                    {
                        Id = id
                    });

            return apartment;
        }

        public async Task<bool> Update(Apartment apartment)
        {
            var affectedRows = await _connection.ExecuteAsync(
                    @"UPDATE apartments SET title = @Title, 
                                           description = @Description, 
                                           address = @Address, 
                                           price_per_day = @PricePerDay,     
                                           num_of_floor = @NumOfFloor, 
                                           square = @Square, 
                                           capacity = @Capacity 
                    WHERE id = @Id",
                    new
                    {
                        Id = apartment.Id,
                        apartment.Title,
                        apartment.Description,
                        apartment.Address,
                        apartment.PricePerDay,
                        apartment.NumOfFloor,
                        apartment.Square,
                        apartment.Capacity
                    });

            return affectedRows > 0;
        }
    }
}
