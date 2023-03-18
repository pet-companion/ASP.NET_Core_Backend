using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareCore.Enum
{
    public enum OrderStatusEnum
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
    }

    public static class OrderStatusExtensions
    {
        public static string ToDisplayName(this OrderStatusEnum OrderStatusEnum)
        {
            return OrderStatusEnum switch
            {
                OrderStatusEnum.Pending => "Pending",
                OrderStatusEnum.Approved => "Approved",
                OrderStatusEnum.Rejected => "Rejected",
                _ => "Unknown Status",
            };
        }
    }
}
