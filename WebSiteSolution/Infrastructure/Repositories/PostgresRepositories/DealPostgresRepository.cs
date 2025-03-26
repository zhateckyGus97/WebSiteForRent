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
                    @"INSERT INTO deals (user_id, apartment_id, check_in_date, check_out_date, total_price, created_at)
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
                    @"DELETE 
                      FROM deals 
                      WHERE id = @Id",
                    new { 
                        Id = id 
                    });

            return affectedRows > 0;
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
                    new { 
                        Id = id 
                    });

            return deal;
        }

        public async Task<bool> Update(Deal deal)
        {
            var affectedRows = await _connection.ExecuteAsync(
                    @"UPDATE deals SET UserId = @UserId,
                                      ApartmentId = @ApartmentId, 
                                      CheckInDate = @CheckInDate, 
                                      CheckOutDate = @CheckOutDate, 
                                      TotalPrice = @TotalPrice, 
                                      UpdatedAt = @UpdatedAt
                    WHERE id = @Id",
                    new
                    {
                        Id = deal.Id,
                        deal.UserId,
                        deal.ApartmentId,
                        deal.CheckInDate,
                        deal.CheckOutDate,
                        deal.TotalPrice,
                        DateTime.Now
                    });

            return affectedRows > 0;
        }
    }
}
