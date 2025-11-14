using Microsoft.EntityFrameworkCore;
using UserService.DAL.Entities;

namespace UserService.DAL
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public UserDbContext()
        {
                
        }

    }
}
