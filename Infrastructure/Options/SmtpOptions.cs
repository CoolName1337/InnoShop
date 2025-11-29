using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Options
{
    public class SmtpOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Email { get; set; }
        public string AppPassword { get; set; }
    }
}
