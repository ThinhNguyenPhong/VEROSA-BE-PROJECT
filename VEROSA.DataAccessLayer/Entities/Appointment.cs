using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Enums;

namespace VEROSA.DataAccessLayer.Entities
{
    public class Appointment
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ServiceId { get; set; }
        public BeautyService Service { get; set; }
        public Guid CustomerId { get; set; }
        public Account Customer { get; set; }
        public Guid ConsultantId { get; set; }
        public Account Consultant { get; set; }
        public DateTime ScheduledAt { get; set; }
        public AppointmentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Payment Payment { get; set; }
    }
}
