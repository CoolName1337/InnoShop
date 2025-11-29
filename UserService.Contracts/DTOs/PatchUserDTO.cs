
using System.ComponentModel.DataAnnotations;

namespace UserService.Contracts.DTOs
{
    public class PatchUserDTO
    {
        [Required]
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
    }
}
