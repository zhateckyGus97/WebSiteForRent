using Dapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Npgsql;

namespace Infrastructure.Repositories.PostgresRepositories
{
    public class ReviewPostgresRepository : IReviewRepository
    {
        private readonly NpgsqlConnection _connection;
        public ReviewPostgresRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Create(Review review)
        {
            var reviewId = await _connection.QuerySingleAsync<int>(
                    @"INSERT INTO reviews (user_id, apartment_id, rating, comment, created_at)
                      VAlUES (@UserId, @ApartmentId, @Rating, @Comment, @CreatedAt)
                      RETURNING Id",
                    new { 
                        review.ApartmentId, 
                        review.UserId, 
                        review.Rating, 
                        review.Comment,
                        CreatedAt = DateTime.Now
                    });
            
            return reviewId;
        }

        public async Task<bool> Delete(int id)
        {
            var affectedRows = await _connection.ExecuteAsync(
                    @"DELETE 
                      FROM reviews 
                      WHERE id = @Id",
                    new { 
                        Id = id 
                    });

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Review>> GetAll()
        {
            var reviews = await _connection.QueryAsync<Review>(
                    @"SELECT 
                        id,
                        apartment_id, 
                        user_id, 
                        rating, 
                        comment, 
                        created_at, 
                        updated_at 
                      FROM reviews"
                    );

            return reviews;
        }

        public async Task<Review?> GetById(int id)
        {
            var review = await _connection.QueryFirstOrDefaultAsync<Review>(
                    @"SELECT 
                        id,
                        apartment_id, 
                        user_id, 
                        rating, 
                        comment, 
                        created_at, 
                        updated_at
                      FROM reviews 
                      WHERE id = @Id", 
                    new { 
                        Id = id 
                    });

            return review;
        }

        public async Task<bool> Update(Review review)
        {
            var affectedRows = await _connection.ExecuteAsync(
                    @"UPDATE reviews SET apartment_id = @ApartmentId,
                                      user_id = @UserId,
                                      rating = @Rating, 
                                      comment = @Comment,
                                      updated_at = @UpdatedAt
                    WHERE id = @Id",
                    new
                    {
                        review.ApartmentId,
                        review.UserId,
                        review.Rating,
                        review.Comment,
                        UpdatedAt = DateTime.Now,
                        review.Id
                    });

            return affectedRows > 0;
        }
    }
}
