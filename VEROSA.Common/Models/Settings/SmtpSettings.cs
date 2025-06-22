using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VEROSA.Common.Models.Settings
{
    public class SmtpSettings
    {
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } = default!;
        public bool EnableSsl { get; set; }
        public string From { get; set; } = "Verosa Beauty";
    }
}
