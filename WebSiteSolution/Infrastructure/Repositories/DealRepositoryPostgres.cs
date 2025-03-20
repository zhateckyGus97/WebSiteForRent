using Dapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Npgsql;

namespace Infrastructure.Repositories
{
    public class DealRepositoryPostgres : IDealRepository
    {
        private readonly NpgsqlConnection _connection;
        public DealRepositoryPostgres(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Create(Deal deal)
        {
            await _connection.OpenAsync();

            var dealId = await _connection.QuerySingleAsync<int>(
                    @"INSERT INTO User (UserId, ApartmentId, CheckInDate, CheckOutDate, TotalPrice, CreatedAt)
                      VAlUES (@UserId, @ApartmentId, @CheckInDate, @CheckOutDate, @TotalPrice, @CreatedAt)
                      RETURNING Id",
                    new { deal.UserId, deal.ApartmentId, deal.CheckInDate, deal.CheckOutDate, deal.TotalPrice, deal.CreatedAt });
            await _connection.CloseAsync();
            return dealId;
        }

        public async Task<bool> Delete(int id)
        {
            await _connection.OpenAsync();

            var affectedRows = await _connection.ExecuteAsync(
                    @"DELETE FROM Deal WHERE id = @Id",
                    new { Id = id });

            await _connection.CloseAsync();

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Deal>> GetAll()
        {
            await _connection.OpenAsync();

            var deals = await _connection.QueryAsync<Deal>(
                    @"SELECT * FROM Deal");

            await _connection.CloseAsync();

            return deals;
        }

        public async Task<Deal?> GetById(int id)
        {
            await _connection.OpenAsync();

            var deal = await _connection.QueryFirstOrDefaultAsync<Deal>(
                    @"SELECT * FROM Deal WHERE id = @Id", new { Id = id });

            await _connection.CloseAsync();

            return deal;
        }

        public async Task<bool> Update(Deal deal)
        {
            await _connection.OpenAsync();

            var affectedRows = await _connection.ExecuteAsync(
                    @"UPDATE User SET UserId = @UserId,
                                      ApartmentId = @ApartmentId, 
                                      CheckInDate = @CheckInDate, 
                                      CheckOutDate = @CheckOutDate, 
                                      TotalPrice = @TotalPrice, 
                                      UpdatedAt = DateTime.Now
                    WHERE id = @Id");

            await _connection.CloseAsync();

            return affectedRows > 0;
        }
    }
}
