using System;
using System.Collections.Generic;
using System.Text;

namespace UserService.DAL.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
