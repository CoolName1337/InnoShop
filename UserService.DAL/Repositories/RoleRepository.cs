using Microsoft.EntityFrameworkCore;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.DAL.Repositories
{
    public class RoleRepository(UserDbContext db) : IRoleRepository
    {
        
        public async Task<Role[]> GetAllRoles()
        {
            return await db.Roles.ToArrayAsync();
        }
        public async Task<Role?> GetRoleById(int roleId)
        {
            return await db.Roles.FindAsync(roleId);
        }

        public async Task<int> GetIdByName(string roleName)
        {
            var role = await db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            return role?.Id ?? 1;
        }
    }
}
