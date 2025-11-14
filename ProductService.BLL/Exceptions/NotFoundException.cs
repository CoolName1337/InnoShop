using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.BLL.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string mes) : base(mes) 
        { 

        }
    }
}
