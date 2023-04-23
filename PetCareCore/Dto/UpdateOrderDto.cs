using PetCareCore.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareCore.Dto
{
    public class UpdateOrderDto
    {
        public int? Id { get; set; }
        public int Qty { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
