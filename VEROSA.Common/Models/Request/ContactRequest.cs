using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEROSA.Common.Models.Request
{
    public class ContactRequest
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string Subject { get; set; } = null!;
        public string Message { get; set; } = null!;
        public bool IsResolved { get; set; }
    }
}
