using System;
using System.Collections.Generic;
using System.Text;

namespace UserService.DAL.Domain
{
    public class UserFilter
    {
        public string? ContainString { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
    }
}
