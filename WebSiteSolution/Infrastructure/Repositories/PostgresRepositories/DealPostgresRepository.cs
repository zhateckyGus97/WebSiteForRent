using Dapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Npgsql;

namespace Infrastructure.Repositories.PostgresRepositories
{
    public class DealPostgresRepository : IDealRepository
    {
        private readonly NpgsqlConnection _connection;
        public DealPostgresRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Create(Deal deal)
        {
            var dealId = await _connection.QuerySingleAsync<int>(
                    @"INSERT INTO Deals (UserId, ApartmentId, CheckInDate, CheckOutDate, TotalPrice, CreatedAt)
                      VAlUES (@UserId, @ApartmentId, @CheckInDate, @CheckOutDate, @TotalPrice, @CreatedAt)
                      RETURNING Id",
                    new { 
                        deal.UserId, 
                        deal.ApartmentId, 
                        deal.CheckInDate, 
                        deal.CheckOutDate, 
                        deal.TotalPrice, 
                        DateTime.Now 
                    });
            
            return dealId;
        }

        public async Task<bool> Delete(int id)
        {
            var affectedRows = await _connection.ExecuteAsync(
                    @"DELETE FROM Deals WHERE id = @Id",
                    new { Id = id });

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Deal>> GetAll()
        {
            var deals = await _connection.QueryAsync<Deal>(
                    @"SELECT Id, UserId, ApartmentId, CheckInDate, CheckOutDate, TotalPrice, CreatedAt, UpdatedAt FROM Deals");

            return deals;
        }

        public async Task<Deal?> GetById(int id)
        {
            var deal = await _connection.QueryFirstOrDefaultAsync<Deal>(
                    @"SELECT Id, UserId, ApartmentId, CheckInDate, CheckOutDate, TotalPrice, CreatedAt, UpdatedAt FROM Deals WHERE id = @Id", new { Id = id });

            return deal;
        }

        public async Task<bool> Update(Deal deal)
        {
            var affectedRows = await _connection.ExecuteAsync(
                    @"UPDATE Deals SET UserId = @UserId,
                                      ApartmentId = @ApartmentId, 
                                      CheckInDate = @CheckInDate, 
                                      CheckOutDate = @CheckOutDate, 
                                      TotalPrice = @TotalPrice, 
                                      UpdatedAt = DateTime.Now
                    WHERE id = @Id");

            return affectedRows > 0;
        }
    }
}
