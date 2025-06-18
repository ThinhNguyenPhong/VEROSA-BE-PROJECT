using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Enums;

namespace VEROSA.DataAccessLayer.Entities
{
    public class Payment
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        public Guid? OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public PaymentMethod Method { get; set; }

        [Required]
        public PaymentStatus Status { get; set; }

        public DateTime PaymentDate { get; set; }
    }
}
