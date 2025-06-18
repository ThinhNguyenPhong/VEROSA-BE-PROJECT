using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEROSA.DataAccessLayer.Entities
{
    public class BeautyService
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
        public ICollection<Image> Images { get; set; }
    }
}
