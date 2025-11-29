using UserService.DAL.Entities;

namespace UserService.DAL.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role[]> GetAllRoles();
        Task<Role?> GetRoleById(int roleId);
        Task<int> GetIdByName(string roleName);
    }
}