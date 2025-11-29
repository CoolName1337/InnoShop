using System.ComponentModel.DataAnnotations;

namespace ProductService.Contracts.DTOs
{
    public class UpdateProductDTO
    {
        [Required]
        public int Id { get; set; }
        [MaxLength(255)]
        public string? Name { get; set; }
        [MaxLength(2000)]
        public string? Description { get; set; }
        [Range(0, int.MaxValue)]
        public int? Count { get; set; }
        [Range(0, double.MaxValue)]
        public decimal? Price { get; set; }
    }
}