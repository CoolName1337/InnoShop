using System.ComponentModel.DataAnnotations;

namespace ProductService.Contracts.DTOs
{
    public class CreateProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
