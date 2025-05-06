using Application.Interfaces;

namespace Application.Services
{
    public class BCryptHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password,
                BCrypt.Net.BCrypt.GenerateSalt(12));

            return hash;
        }

        public bool VerifyPassword(string password, string? storedHash)
        {
            if (string.IsNullOrEmpty(storedHash))
                return false;

            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }

    }
}
