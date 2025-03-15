using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTO
{
    public class ApartmentDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public double PricePerDay { get; set; }
        public int NumOfFloor { get; set; }
        public double Square { get; set; }
        public int Capacity { get; set; }
    }
}