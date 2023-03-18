using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareData.Models
{
    public class BaseClass
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public BaseClass()
        {
            CreatedAt = DateTime.Now;
        }
    }
}   
