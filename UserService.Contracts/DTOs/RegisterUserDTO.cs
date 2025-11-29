using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UserService.Contracts.DTOs
{
    public class RegisterUserDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        //[RegularExpression(@"^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*]).+$",
        //    ErrorMessage = "Password must contain an uppercase letter, a number, and a special character")]
        public string Password { get; set; }
    }
}
