using Dapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Npgsql;

namespace Infrastructure.Repositories
{
    public class ReviewRepositoryPostgres : IReviewRepository
    {
        private readonly NpgsqlConnection _connection;
        public ReviewRepositoryPostgres(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Create(Review review)
        {
            await _connection.OpenAsync();

            var reviewId = await _connection.QuerySingleAsync<int>(
                    @"INSERT INTO Review (ApartmentId, UserId, Rating, Comment, CreatedAt)
                      VAlUES (@ApartmentId, @UserId, @Rating, @Comment, @CreatedAt)
                      RETURNING Id",
                    new { review.ApartmentId, review.UserId, review.Rating, review.Comment, review.CreatedAt });
            await _connection.CloseAsync();
            return reviewId;
        }

        public async Task<bool> Delete(int id)
        {
            await _connection.OpenAsync();

            var affectedRows = await _connection.ExecuteAsync(
                    @"DELETE FROM Review WHERE id = @Id",
                    new { Id = id });

            await _connection.CloseAsync();

            return affectedRows > 0;
        }

        public async Task<IEnumerable<Review>> GetAll()
        {
            await _connection.OpenAsync();

            var users = await _connection.QueryAsync<Review>(
                    @"SELECT ID, ApartmentId, UserId, Rating, Comment, CreatedAt, UpdatedAt FROM Review");

            await _connection.CloseAsync();

            return users;
        }

        public async Task<Review?> GetById(int id)
        {
            await _connection.OpenAsync();

            var review = await _connection.QueryFirstOrDefaultAsync<Review>(
                    @"SELECT Id, ApartmentId, UserId, Rating, Comment, CreatedAt, UpdatedAt FROM Review WHERE id = @Id", new { Id = id });

            await _connection.CloseAsync();

            return review;
        }

        public async Task<bool> Update(Review review)
        {
            await _connection.OpenAsync();

            var affectedRows = await _connection.ExecuteAsync(
                    @"UPDATE Review SET ApartmentId = @ApartmentId,
                                      UserId = @UserId,
                                      Rating = @Rating, 
                                      Comment = @Comment,
                                      UpdatedAt = DateTime.Now
                    WHERE id = @Id");

            await _connection.CloseAsync();

            return affectedRows > 0;
        }
    }
}
