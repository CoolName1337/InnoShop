using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Options
{
    public class JwtOptions
    {
        public string SecretKey { get; set; }
        public int ExpireHours { get; set; }
        public string SecretKeyForEmailConfirmation { get; set; }
        public int ExpireMinutesForEmailConfirmation { get; set; }
        public string SecretKeyForPasswordConfirmation { get; set; }
        public int ExpireMinutesForPasswordConfirmation { get; set; }

    }
}
