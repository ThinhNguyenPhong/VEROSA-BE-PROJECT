using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Enums;

namespace VEROSA.DataAccessLayer.Entities
{
    public class DiscountCode
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string Code { get; set; }

        [Required]
        public DiscountType Type { get; set; }

        [Required]
        public decimal Value { get; set; }

        public int? UsageLimit { get; set; }
        public int UsageCount { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
