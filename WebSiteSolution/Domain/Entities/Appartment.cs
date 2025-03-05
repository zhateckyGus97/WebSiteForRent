using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Appartment
    {
        public int Id { get; set; }
        public int DealId { get; set; }
        public int AppartmentType { get; set; }
        public int NumOfFloor { get; set; }
        public int Square { get; set; }
        public int Capacity { get; set; }
    }
}
