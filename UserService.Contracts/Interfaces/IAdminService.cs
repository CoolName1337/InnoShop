using UserService.Contracts.DTOs;

namespace UserService.Contracts.Interfaces
{
    public interface IAdminService
    {
        Task DeleteUserAsync(int userId, CancellationToken ct = default);
        Task<List<UserDTO>> GetAllAsync(bool includeInactive, CancellationToken ct = default);
        Task<UserDTO> GetByIdAsync(int userId, CancellationToken ct = default);
        Task<UserDTO> UpdateUserAsync(PatchUserDTO patchUser, CancellationToken ct = default);
    }
}