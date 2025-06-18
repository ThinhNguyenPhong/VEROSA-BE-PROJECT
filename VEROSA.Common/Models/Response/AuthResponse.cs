using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEROSA.Common.Models.Response
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public string Role { get; set; }
    }
}
