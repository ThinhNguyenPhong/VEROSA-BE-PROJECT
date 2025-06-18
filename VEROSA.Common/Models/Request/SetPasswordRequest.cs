using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEROSA.Common.Models.Request
{
    public class SetPasswordRequest
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
