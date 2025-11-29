using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace UserService.DAL
{
    public class UserDbContextFactory : IDesignTimeDbContextFactory<UserDbContext>
    {
        public UserDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserDbContext>();

            optionsBuilder.UseSqlServer(
                "Data Source=localhost\\SQLEXPRESS01;Database=UsersDb;Integrated Security=True;Persist Security Info=False;" +
                "Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Command Timeout=30"
                );

            return new UserDbContext(optionsBuilder.Options);
        }
    }
}
