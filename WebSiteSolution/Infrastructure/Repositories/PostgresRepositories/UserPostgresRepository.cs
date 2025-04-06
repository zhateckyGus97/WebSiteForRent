using Domain.Entities;
using Infrastructure.Interfaces;
using Npgsql;
using Dapper;

namespace Infrastructure.Repositories.PostgresRepositories
{
    public class UserPostgresRepository : IUserRepository
    {
        private readonly NpgsqlConnection _connection;
        public UserPostgresRepository(NpgsqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<int> Create(User user)
        {
            var userId = await _connection.QuerySingleAsync<int>(
                    @"INSERT INTO users (full_name, email, phone_number, role, passport, date_of_birth)
                      VAlUES (@Fullname, @Email, @PhoneNumber, @Role, @Passport, @DateOfBirth)
                      RETURNING Id",
                    new { user.FullName, 
                        user.Email, 
                        user.PhoneNumber, 
                        user.Role, 
                        user.Passport, 
                        user.DateOfBirth 
                    });
            
            return userId;
        }

        public async Task<bool> Delete(int id)
        {
            var affectedRows = await _connection.ExecuteAsync(
                    @"DELETE 
                      FROM users 
                      WHERE id = @Id",
                    new { 
                        Id = id 
                    });

            return affectedRows > 0;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await _connection.QueryAsync<User>(
                    @"SELECT 
                        id, 
                        full_name,  
                        email,  
                        phone_number,  
                        role, passport,  
                        date_of_birth  
                      FROM users"
                    );

            return users;
        }

        public async Task<User?> GetById(int id)
        {
            var user = await _connection.QueryFirstOrDefaultAsync<User>(
                    @"SELECT 
                        id, 
                        full_name,  
                        email,  
                        phone_number,  
                        role, passport,  
                        date_of_birth  
                      FROM users
                      WHERE id = @Id", 
                    new { 
                        Id = id 
                    });

            return user;
        }

        public async Task<bool> Update(User user)
        {
            var affectedRows = await _connection.ExecuteAsync(
                    @"UPDATE Users SET full_name = @FullName,
                                      email = @Email,
                                      phone_number = @PhoneNumber, 
                                      role = @Role,
                                      passport = @Passport,
                                      date_of_birth = @DateOfBirth
                    WHERE id = @Id",
                    new
                    {
                        Id = user.Id,
                        user.FullName,
                        user.Email,
                        user.PhoneNumber,
                        user.Role,
                        user.Passport,
                        user.DateOfBirth
                    });

            return affectedRows > 0;
        }
    }
}
