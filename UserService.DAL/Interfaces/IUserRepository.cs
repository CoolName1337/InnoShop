using UserService.DAL.Domain;
using UserService.DAL.Entities;

namespace UserService.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task SetActiveAsync(int userId, bool isActive, CancellationToken cancellationToken);
        Task<User> CreateAsync(User user, CancellationToken cancellationToken);
        Task DeleteAsync(int id, CancellationToken cancellationToken);
        Task<List<User>> GetAllAsync(int[] userIds, CancellationToken cancellationToken = default, bool includeInactive = false);
        Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default, bool includeInactive = false);
        Task<List<User>> GetAllByFilterAsync(UserFilter userFilter, CancellationToken cancellationToken);
        Task<User?> GetByEmailAsync(string userEmail, CancellationToken ct);
        Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<(List<User>, int)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken, bool includeInactive = false);
        Task<User> UpdateAsync(User user, CancellationToken cancellationToken);
    }
}