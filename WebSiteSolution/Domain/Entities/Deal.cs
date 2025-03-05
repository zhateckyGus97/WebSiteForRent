using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Deal
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AppartmentId { get; set; }
        public DateTime StartRent { get; set; }
        public DateTime FinishRent { get; set; }
    }
}
