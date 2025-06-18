using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEROSA.Common.Models.Request
{
    public class LoginRequest
    {
        public string UsernameOrEmail { get; set; }
        public string Password { get; set; }
    }
}
