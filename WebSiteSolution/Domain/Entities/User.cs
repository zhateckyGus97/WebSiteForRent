using Domain.Enums;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public UserRoles Role { get; set; }
        public string Passport { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int? LogoAttacmentId { get; set; }
        public string? PasswordHash { get; set; }
    }
}
