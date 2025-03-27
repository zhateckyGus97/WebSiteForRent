namespace Domain.Entities
{
    public class Deal
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ApartmentId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public double TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
