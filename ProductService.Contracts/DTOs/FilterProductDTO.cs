using System.ComponentModel.DataAnnotations;

namespace ProductService.Contracts.DTOs
{
    public class FilterProductDTO
    {
        public string? ContainString { get; set; }
        [Range(0, double.MaxValue)]
        public decimal? MinPrice { get; set; }
        [Range(0, double.MaxValue)]
        public decimal? MaxPrice { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
        public int? OwnerId { get; set; }
    }
}
