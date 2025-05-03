using Domain.Entities;
using Infrastructure.Interfaces;
using Npgsql;
using Dapper;
using Infrastructure.Database.TypeMappings;

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
            const string query =
                    @"INSERT INTO users (full_name, email, phone_number, role, passport, date_of_birth, password_hash)
                      VAlUES (@Fullname, @Email, @PhoneNumber, @Role::user_role, @Passport, @DateOfBirth, @PasswordHash)
                      RETURNING Id"/*,
                    new { 
                        user.FullName, 
                        user.Email, 
                        user.PhoneNumber, 
                        user.Role, 
                        user.Passport, 
                        user.DateOfBirth,
                        user.PasswordHash
                    }*/;

            return await _connection.ExecuteScalarAsync<int>(query, user.AsDapperParams());
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

        public async Task<User?> GetByEmail(string email)
        {
            const string query = @"SELECT 
                        id, 
                        full_name,  
                        email,  
                        phone_number,  
                        role, 
                        passport,  
                        date_of_birth,
                        password_hash
                      FROM users
                      WHERE Email = @Email";

            return await _connection.QuerySingleOrDefaultAsync<User>(query, new { Email = email });
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
            const string query =  @"UPDATE Users SET full_name = @FullName,
                                      email = @Email,
                                      phone_number = @PhoneNumber, 
                                      role = @Role,
                                      passport = @Passport,
                                      date_of_birth = @DateOfBirth,
                                      password_hash = @PasswordHash
                    WHERE id = @Id";
                /*,
                    new
                    {
                        Id = user.Id,
                        user.FullName,
                        user.Email,
                        user.PhoneNumber,
                        user.Role,
                        user.Passport,
                        user.DateOfBirth,
                        user.PasswordHash
                    }*/

            var affectedRows = await _connection.ExecuteAsync(query, user.AsDapperParams());
            return affectedRows > 0;
        }
    }
}
