using PetCareCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareCore.Dto
{
    public class PetVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Weight { get; set; }
        public int UserId { get; set; }
        public int BreedId { get; set; }
        public FileData PetImage { get; set; }
    }
}
