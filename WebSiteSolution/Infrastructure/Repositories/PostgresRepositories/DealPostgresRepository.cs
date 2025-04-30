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
            deal.CreatedAt = DateTime.Now;
            deal.UpdatedAt = DateTime.Now;
            var dealId = await _connection.QuerySingleAsync<int>(
                    @"INSERT INTO deals (user_id, apartment_id, check_in_date, check_out_date, total_price, created_at)
                      VAlUES (@UserId, @ApartmentId, @CheckInDate, @CheckOutDate, @TotalPrice, @CreatedAt)
                      RETURNING Id",
                    deal);

            return dealId;
        }

        public async Task<bool> Delete(int id)
        {
            var affectedRows = await _connection.ExecuteAsync(
                    @"DELETE 
                      FROM deals 
                      WHERE id = @Id",
                    new
                    {
                        Id = id
                    });

            return affectedRows > 0;
        }

        public async Task DeleteByApartmentId(int apartmentid)
        {
            await _connection.ExecuteAsync("DELETE FROM deals WHERE apartment_id = @ApartmentId", new { ApartmentId = apartmentid });
        }

        public async Task DeleteByUserId(int userid)
        {
            await _connection.ExecuteAsync("DELETE FROM deals WHERE user_id = @UserId", new { UserId = userid });
        }

        public async Task<IEnumerable<Deal>> GetAll()
        {
            var deals = await _connection.QueryAsync<Deal>(
                    @"SELECT 
                        id,
                        user_id, 
                        apartment_id, 
                        check_in_date, 
                        check_out_date, 
                        total_price, 
                        created_at, 
                        updated_at 
                      FROM deals"
                    );

            return deals;
        }

        public async Task<Deal?> GetById(int id)
        {
            var deal = await _connection.QueryFirstOrDefaultAsync<Deal>(
                    @"SELECT 
                        id,
                        user_id, 
                        apartment_id, 
                        check_in_date, 
                        check_out_date, 
                        total_price, 
                        created_at, 
                        updated_at 
                      FROM deals 
                      WHERE id = @Id",
                    new
                    {
                        Id = id
                    });

            return deal;
        }

        public async Task<bool> Update(Deal deal)
        {
            deal.UpdatedAt = DateTime.Now;
            var affectedRows = await _connection.ExecuteAsync(
                    @"UPDATE deals SET user_id = @UserId,
                                      apartment_id = @ApartmentId, 
                                      check_in_date = @CheckInDate, 
                                      check_out_date = @CheckOutDate, 
                                      total_price = @TotalPrice, 
                                      updated_at = @UpdatedAt
                    WHERE id = @Id",
                    deal);

            return affectedRows > 0;
        }
    }
}
