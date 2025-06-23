using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEROSA.Common.Enums;

namespace VEROSA.Common.Models.Request
{
    public class SupportTicketRequest
    {
        public Guid CustomerId { get; set; }
        public string Subject { get; set; } = null!;
        public string? Message { get; set; }
        public SupportStatus Status { get; set; }
    }
}
