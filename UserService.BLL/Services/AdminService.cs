using AutoMapper;
using UserService.BLL.Exceptions;
using UserService.Contracts.DTOs;
using UserService.DAL.Interfaces;
using UserService.Contracts.Interfaces;
using Infrastructure;

namespace UserService.BLL.Services
{
    public class AdminService(IUserRepository userRepository, IMapper mapper, MyHttpClient httpClient) : IAdminService
    {
        public async Task<UserDTO> UpdateUserAsync(PatchUserDTO patchUser, CancellationToken ct = default)
        {
            var user = await userRepository.GetByIdAsync(patchUser.Id, ct) ??
                throw new NotFoundUserException($"Can't update user because user with id = {patchUser.Id} is not exist");

            mapper.Map(patchUser, user);

            await userRepository.UpdateAsync(user, ct);

            await httpClient.SoftDelete(user.Id, !user.IsActive);

            return mapper.Map<UserDTO>(user);
        }

        public async Task DeleteUserAsync(int userId, CancellationToken ct = default)
        {
            var user = await userRepository.GetByIdAsync(userId, ct) ??
                throw new NotFoundUserException($"Can't delete user because user with id = {userId} is not exist");

            await userRepository.DeleteAsync(userId, ct);
        }

        public async Task<List<UserDTO>> GetAllAsync(bool includeInactive, CancellationToken ct = default)
        {
            var users = await userRepository.GetAllAsync(ct, includeInactive);
            
            return mapper.Map<List<UserDTO>>(users);
        }

        public async Task<UserDTO> GetByIdAsync(int userId, CancellationToken ct = default)
        {
            var user = await userRepository.GetByIdAsync(userId, ct) ??
                throw new NotFoundUserException($"Can't find user because user with id = {userId} is not exist");

            return mapper.Map<UserDTO>(user);
        }
    }
}
