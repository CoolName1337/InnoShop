using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.DAL.Domain
{
    public class ProductFilter
    {
        public string? ContainString { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public int? OwnerId { get; set; }
    }
}
