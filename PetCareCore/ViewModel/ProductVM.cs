using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareCore.ViewModel
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }
        public int CategoryId { get; set; }
        public int StoreId { get; set; }
        public int UserId { get; set; }
        public FileData ProductImg { get; set; }
        public string ImgName { get; set; }
    }
}
