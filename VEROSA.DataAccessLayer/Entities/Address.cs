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
        public string State { get; set; }

        [MaxLength(100)]
        public string Country { get; set; }

        [MaxLength(20)]
        public string PostalCode { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
