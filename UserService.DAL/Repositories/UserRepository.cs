using Microsoft.EntityFrameworkCore;
using UserService.DAL.Domain;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.DAL.Repositories
{
    public class UserRepository(UserDbContext db) : IUserRepository
    {
        public async Task<User?> GetByEmailAsync(string userEmail, CancellationToken ct)
        {
            return await db.Users.AsNoTracking()
                .Include(u=>u.Role) 
                    .ThenInclude(r=>r.RolePermissions)
                        .ThenInclude(rp => rp.Permission)
                .FirstOrDefaultAsync(user => user.Email == userEmail);
        }

        public async Task<User> CreateAsync(User user, CancellationToken cancellationToken)
        {
            await db.Users.AddAsync(user, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var user = await db.Users.FindAsync(id, cancellationToken);
            if (user != null)
            {
                db.Users.Remove(user);
                await db.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken, bool includeInactive)
        {
            var query = db.Users.AsQueryable();
            if (!includeInactive)
                query = query.Where(user => user.IsActive);

            return await query.AsNoTracking().ToListAsync(cancellationToken);
        }


        public async Task<List<User>> GetAllAsync(int[] userIds, CancellationToken cancellationToken, bool includeInactive)
        {
            var query = db.Users.AsQueryable();
            if (!includeInactive)
                query = query.Where(user => user.IsActive);
            query = query.Where(u => userIds.Contains(u.Id));

            return await query.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<List<User>> GetAllByFilterAsync(UserFilter userFilter, CancellationToken cancellationToken)
        {
            var query = db.Users.AsQueryable();

            if (!string.IsNullOrEmpty(userFilter.ContainString))
                query = query.Where(user => user.Name.Contains(userFilter.ContainString));

            if (userFilter.FromDate is DateOnly fromDate)
                query = query.Where(user => user.DateOfCreating >= fromDate);
            if (userFilter.ToDate is DateOnly toDate)
                query = query.Where(user => user.DateOfCreating <= toDate);

            return await query.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await db.Users.FindAsync(id, cancellationToken);
        }

        public async Task<User> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            db.Users.Update(user);
            await db.SaveChangesAsync(cancellationToken);
            return user;
        }

        public async Task SetActiveAsync(int userId,bool isActive, CancellationToken cancellationToken)
        {
            var user = await db.Users.FindAsync(userId, cancellationToken);
            if (user != null)
            {
                user.IsActive = isActive;
                db.Users.Update(user);
                await db.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<(List<User>, int)> GetPagedAsync(
            int pageNumber, int pageSize, CancellationToken cancellationToken, bool includeInactive = false)
        {
            var query = db.Users.AsNoTracking().AsQueryable();

            if (!includeInactive)
                query = query.Where(user => !user.IsActive);

            var totalCount = await query.CountAsync(cancellationToken);

            var users = await query
                .OrderBy(user => user.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (users, totalCount);
        }
    }
}
