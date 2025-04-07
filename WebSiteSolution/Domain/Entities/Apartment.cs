namespace Domain.Entities
{
    public class Apartment
    {
        public int Id { get; set; }
        public int Owner_id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public double PricePerDay { get; set; }
        public int NumOfFloor { get; set; }
        public double Square { get; set; }
        public int Capacity { get; set; }
    }
}
