using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.Contracts.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateOnly CreatedDate { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public int OwnerId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
