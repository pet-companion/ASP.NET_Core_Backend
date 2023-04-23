using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareCore.Dto
{
    public class AddProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }
        public int CategoryId { get; set; }
        public int StoreId { get; set; }
        public int UserId { get; set; }
        public IFormFile ProductImg { get; set; }
    }
}
