using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCareData.Models
{
    public class Pet : BaseClass
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Weight { get; set; }
        public string ImgName { get; set; }
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public User User { get; set; }
        [ForeignKey(nameof(Breed))]
        public int BreedId { get; set; }
        public Breed Breed { get; set; }
    }
}
