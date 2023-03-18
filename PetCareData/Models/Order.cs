using PetCareCore.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareData.Models
{
    public class Order : BaseClass
    {
        [Key]
        public int Id { get; set; }
        public string PriceOnDemand { get; set; }
        public string Qty { get; set; }
        public OrderStatusEnum Status { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
