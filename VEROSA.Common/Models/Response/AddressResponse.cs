using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEROSA.Common.Models.Response
{
    public class AddressResponse
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Street { get; set; } = null!;
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Country { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
