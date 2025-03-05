using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AppartmentType
    {
        public int TypeId { get; set; }
        public string Description { get; set; }
        public string TypeName { get; set; }
        public int PricePerDay { get; set; }
        public int Deposit { get; set; }
    }
}
