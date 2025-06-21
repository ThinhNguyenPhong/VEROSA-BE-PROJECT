using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEROSA.DataAccessLayer.Entities
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }

        [Required, MaxLength(200)]
        public string Street { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(100)]
        public string District { get; set; }

        [MaxLength(100)]
        public string Country { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
