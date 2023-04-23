using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareCore.Enum
{
    public enum StatusMessageEnum
    {
        Success = 1,
        Failed
    }

    public static class StatusMessageExtensions
    {
        public static string ToDisplayName(this StatusMessageEnum StatusMessageEnum)
        {
            return StatusMessageEnum switch
            {
                StatusMessageEnum.Success => "Success",
                StatusMessageEnum.Failed => "Failed",
                _ => "Unknown Status",
            };
        }
    }
}
