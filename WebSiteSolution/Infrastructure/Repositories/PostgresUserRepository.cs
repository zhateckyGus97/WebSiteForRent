using Domain.Entities;
using Infrastructure.Interfaces;
using Npgsql;
using Dapper;
using System.Data;

namespace Infrastructure.Repositories
{
    public class PostgresUserRepository : IUserRepository
    {
        private readonly NpgsqlConnection _connection;
        public PostgresUserRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Create(User user)
        {
            await _connection.OpenAsync();

            var userId = await _connection.QuerySingleAsync<int>(
                    @"INSERT INTO User (FullName, Email, PhoneNumber, Role, Passport, DateOfBirth)
                      VAlUES (@Fullname, @Email, @PhoneNumber, @Role, @Passport, @DateOfBirth)
                      RETURNING Id", 
                    new { user.FullName, user.Email, user.PhoneNumber, user.Role, user.Passport, user.DateOfBirth });
            await _connection.CloseAsync();
            return userId;
        }

        public async Task<bool> Delete(int id)
        {
            await _connection.OpenAsync();

            var affectedRows = await _connection.ExecuteAsync(
                    @"DELETE FROM User WHERE id = @Id",
                    new { Id = id });

            await _connection.CloseAsync();

            return affectedRows > 0;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            await _connection.OpenAsync();

            var users = await _connection.QueryAsync<User>(
                    @"SELECT * FROM User");

            await _connection.CloseAsync();

            return users;
        }

        public async Task<User?> GetById(int id)
        {
            await _connection.OpenAsync();

            var user = await _connection.QueryFirstOrDefaultAsync<User>(
                    @"SELECT * FROM User WHERE id = @Id", new { Id = id });

            await _connection.CloseAsync();

            return user;
        }

        public async Task<bool> Update(User user)
        {
            await _connection.OpenAsync();

            var affectedRows = await _connection.ExecuteAsync(
                    @"UPDATE User SET FullName = @FullName,
                                      Email = @Email,
                                      PhoneNumber = @PhoneNumber, 
                                      Role = @Role,
                                      Passport = @Passport,
                                      DateOfBirth = @DateOfBirth
                                      
                    WHERE id = @Id");

            await _connection.CloseAsync();

            return affectedRows > 0;
        }
    }
}
