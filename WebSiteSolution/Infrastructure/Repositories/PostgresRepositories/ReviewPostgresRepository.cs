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
                    @"INSERT INTO Reviews (ApartmentId, UserId, Rating, Comment, CreatedAt)
                      VAlUES (@ApartmentId, @UserId, @Rating, @Comment, @CreatedAt)
                      RETURNING Id",
                    new { 
                        review.ApartmentId, 
                        review.UserId, 
                        review.Rating, 
                        review.Comment, 
                        DateTime.Now 
                    });
            
            return reviewId;
        }

        public async Task<bool> Delete(int id)
        {
            var affectedRows = await _connection.ExecuteAsync(
                    @"DELETE FROM Reviews WHERE id = @Id",
                    new { Id = id });

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Review>> GetAll()
        {
            var users = await _connection.QueryAsync<Review>(
                    @"SELECT ID, ApartmentId, UserId, Rating, Comment, CreatedAt, UpdatedAt FROM Reviews");

            return users;
        }

        public async Task<Review?> GetById(int id)
        {
            var review = await _connection.QueryFirstOrDefaultAsync<Review>(
                    @"SELECT Id, ApartmentId, UserId, Rating, Comment, CreatedAt, UpdatedAt FROM Reviews WHERE id = @Id", new { Id = id });

            return review;
        }

        public async Task<bool> Update(Review review)
        {
            var affectedRows = await _connection.ExecuteAsync(
                    @"UPDATE Reviews SET ApartmentId = @ApartmentId,
                                      UserId = @UserId,
                                      Rating = @Rating, 
                                      Comment = @Comment,
                                      UpdatedAt = DateTime.Now
                    WHERE id = @Id");

            return affectedRows > 0;
        }
    }
}
