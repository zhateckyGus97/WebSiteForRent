namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Passport { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
