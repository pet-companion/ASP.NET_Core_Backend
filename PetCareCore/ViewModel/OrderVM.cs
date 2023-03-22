using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareCore.ViewModel
{
    public class OrderVM
    {
        public int Id { get; set; }
        public double PriceOnDemand { get; set; }
        public int Qty { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
