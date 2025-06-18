using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Enums;

namespace VEROSA.DataAccessLayer.Entities
{
    public class SupportTicket
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Account Customer { get; set; }

        [Required]
        public string Subject { get; set; }
        public string Message { get; set; }
        public SupportStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
