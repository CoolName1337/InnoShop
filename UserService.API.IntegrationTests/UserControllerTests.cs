using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using UserService.Contracts.DTOs;
using UserService.DAL;
using UserService.DAL.Entities;

namespace UserService.API.IntegrationTests
{
    public class UserControllerTests
    {
        private List<User> _users;
        private HttpClient _client;
        private WebApplicationFactory<Program> _webHost;
        private IJwtProvider _jwtProvider;
        private IPasswordHasher _passwordHasher;
        private List<string> _passwords;

        [OneTimeSetUp]
        public void OneSetup()
        {
            _webHost = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var descriptors = services.Where(
                        d => d.ServiceType == typeof(DbContextOptions<UserDbContext>) || d.ServiceType == typeof(UserDbContext)).ToList();

                    foreach (var descriptor in descriptors)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<UserDbContext>(
                        options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });
                });
            });

            _jwtProvider = _webHost.Services.CreateScope().ServiceProvider.GetRequiredService<IJwtProvider>();
            _passwordHasher = _webHost.Services.CreateScope().ServiceProvider.GetRequiredService<IPasswordHasher>();

            _passwords = [
                "shi1",
                "shi2",
                "shi3",
                "shi4"
            ];
            
        }
        [SetUp]
        public async Task Setup()
        {
            UserDbContext dbContext = _webHost.Services.CreateScope().ServiceProvider.GetRequiredService<UserDbContext>();

            dbContext.Users.RemoveRange(dbContext.Users);

            await dbContext.SaveChangesAsync();
            _users = [
                new (){
                    Name = "u1",
                    Id = 1,
                    Email = "chel1@gmail.com",
                    PasswordHash = _passwordHasher.Generate(_passwords[0]),
                    IsActive = true,
                    IsEmailConfirmed = true,
                    RoleId = 2,
                    Role = new Role(){Name = "Admin"},
                    DateOfCreating = DateOnly.FromDateTime(DateTime.Now)
                },
                new (){
                    Name = "u2",
                    Id = 2,
                    Email =  "chel2@gmail.com",
                    PasswordHash = _passwordHasher.Generate(_passwords[1]),
                    IsActive = true,
                    IsEmailConfirmed = true,
                    RoleId = 1,
                    Role = new Role(){Name = "User"},
                    DateOfCreating = DateOnly.FromDateTime(DateTime.Now)
                },
                new (){
                    Name = "u3",
                    Id = 3,
                    Email =  "chel3@gmail.com",
                    PasswordHash = _passwordHasher.Generate(_passwords[2]),
                    IsActive = true,
                    IsEmailConfirmed = false,
                    RoleId = 1,
                    Role = new Role(){ Name = "User" },
                    DateOfCreating = DateOnly.FromDateTime(DateTime.Now)
                },
                new (){
                    Name = "u4",
                    Id = 4,
                    Email =  "chel4@gmail.com",
                    PasswordHash = _passwordHasher.Generate(_passwords[3]),
                    IsActive = false,
                    IsEmailConfirmed = true,
                    RoleId = 1,
                    Role = new Role(){Name = "User"},
                    DateOfCreating = DateOnly.FromDateTime(DateTime.Now)
                }];

            await dbContext.Users.AddRangeAsync(_users);
            await dbContext.SaveChangesAsync();

            _client = _webHost.CreateClient();
        }

        [Test]  
        public async Task GetAll_SendRequest_ReturnsAllActiveUsers()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/user/all");

            var users = await response.Content.ReadFromJsonAsync<List<User>>();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(users.Select(u => u.Id), Is.EquivalentTo(_users.Where(u=>u.IsActive).Select(u => u.Id)));
        }
        [Test]
        public async Task Register_SendAlreadyExistedEmail_ReturnsBadRequest()
        {
            var registerDTO = new RegisterUserDTO()
            {
                Email = "chel1@gmail.com",
                Name = "fasf",
                Password = "shi5"
            };
            HttpResponseMessage response = await _client.PostAsJsonAsync("/api/user/register", registerDTO);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }
        [Test]
        public async Task Login_SendRequest_ReturnsResponseWithCookies()
        {
            var loginDTO = new LoginUserDTO()
            {
                Email = _users[0].Email,
                Password = _passwords[0]
            };

            HttpResponseMessage response = await _client.PostAsJsonAsync("/api/user/login", loginDTO);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            if (response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> cookies))
            {
                var cookie = ParseCookies(cookies);

                Assert.That(cookie.ContainsKey("nyam-nyam"));
                if (cookie.ContainsKey("nyam-nyam"))
                {
                    string cookieValue = cookie["nyam-nyam"];

                    var user = _users[0];
                    var token = _jwtProvider.GenerateToken(user.Email, user.Id.ToString(), user.Role.Name);

                    Assert.That(cookieValue, Is.EqualTo(token));
                }
            }
        }
        private static Dictionary<string, string> ParseCookies(IEnumerable<string> setCookieHeaders)
        {
            return setCookieHeaders
                .Select(h => h.Split(';')[0])
                .Select(nv => nv.Split('='))
                .ToDictionary(parts => parts[0], parts => parts[1]);
        }
    }
}
