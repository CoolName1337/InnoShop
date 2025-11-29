using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.DAL.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateOnly CreatedDate { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public int OwnerId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
