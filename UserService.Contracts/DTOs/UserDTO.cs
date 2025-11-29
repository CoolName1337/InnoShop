using System;
using System.Collections.Generic;
using System.Text;

namespace UserService.Contracts.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public bool IsActive { get; set; }

        public DateOnly DateOfCreating { get; set; }
    }
}
