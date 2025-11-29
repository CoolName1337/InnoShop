using System.ComponentModel.DataAnnotations;

namespace UserService.Contracts.DTOs
{
    public class ResetPasswordDTO
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string Token { get; set; }
    }
}
