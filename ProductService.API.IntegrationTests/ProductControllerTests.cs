using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Contracts.DTOs;
using ProductService.DAL;
using ProductService.DAL.Entities;
using System.Net;
using System.Net.Http.Json;
using UserService.DAL.Entities;

namespace ProductService.API.IntegrationTests
{
    [TestFixture]
    public class ProductControllerTests
    {
        private List<Product> _products;
        private HttpClient _client;
        private WebApplicationFactory<Program> _webHost;
        private IJwtProvider _jwtProvider;

        [OneTimeSetUp]
        public void OneSetup() {

            _webHost = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var descriptors = services.Where(
                        d => d.ServiceType == typeof(DbContextOptions<ProductDbContext>) || d.ServiceType == typeof(ProductDbContext)).ToList();

                    foreach (var descriptor in descriptors)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<ProductDbContext>(
                        options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });
                });
            });
            _jwtProvider = _webHost.Services.CreateAsyncScope().ServiceProvider.GetRequiredService<IJwtProvider>();

        }
        [SetUp]
        public async Task Setup()
        {
            ProductDbContext dbContext = _webHost.Services.CreateScope().ServiceProvider.GetRequiredService<ProductDbContext>();

            dbContext.Products.RemoveRange(dbContext.Products);

            await dbContext.SaveChangesAsync();

            _products = [
                new (){
                    Name = "p1",
                    Count = 1,
                    CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    Price = 10,
                    OwnerId = 1,
                    Id = 1
                },
                new (){
                    Name = "p2",
                    Count = 2,
                    CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    Price = 20,
                    OwnerId = 2,
                    Id = 2
                },
                new (){
                    Name = "p3",
                    Count = 3,
                    CreatedDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    Price = 30,
                    OwnerId = 3,
                    Id = 3
                }];
            await dbContext.Products.AddRangeAsync(_products);
            await dbContext.SaveChangesAsync();

            _client = _webHost.CreateClient();
        }

        [Test]
        public async Task GetAllAsync_SendRequest_ReturnsAllProducts()
        {
            HttpResponseMessage response = await _client.GetAsync("/api/product");

            var products = await response.Content.ReadFromJsonAsync<List<Product>>();

            Console.WriteLine(products.Count);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(products.Select(p => p.Id), Is.EquivalentTo(_products.Select(p => p.Id)));
        }
        [Test]
        public async Task GetAsync_SendIncorrectId_ReturnsNotFound()
        {
            int id = -1;
            HttpResponseMessage response = await _client.GetAsync($"/api/product/{id}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        public async Task GetByOwnerAsync_SendOwnerId_ReturnsProductsByOwnerId()
        {
            int id = 1;
            var productsWithOwnerId1 = _products.Where(p => p.OwnerId == id);
            HttpResponseMessage response = await _client.GetAsync($"/api/product/owner?ownerId={id}");

            var resProducts = await response.Content.ReadFromJsonAsync<List<Product>>();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(resProducts.Select(p => p.OwnerId), Is.EquivalentTo(productsWithOwnerId1.Select(p=>p.OwnerId)));
        }
        [Test]
        public async Task GetByFilterAsync_SendFilter_ReturnsProductsCorrectly()
        {
            var filter = new FilterProductDTO()
            {
                ContainString = "u",
                MinPrice = 10,
                MaxPrice = 20
            };
            var productsByFilter = _products.Where(p => 
                (p.Name.Contains(filter.ContainString)||p.Description.Contains(filter.ContainString))&&
                (p.Price >= filter.MinPrice && p.Price <= filter.MaxPrice)
            );
            HttpResponseMessage response = await _client.PostAsJsonAsync($"/api/product/search", filter);

            var resProducts = await response.Content.ReadFromJsonAsync<List<Product>>();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(resProducts.Select(p => p.OwnerId), Is.EquivalentTo(productsByFilter.Select(p=>p.OwnerId)));
        }
        [Test]
        public async Task PostAsync_SendRequestWithoutAuthToken_ReturnsUnauthorized()
        {
            var createProductDto = new CreateProductDTO()
            {
                Name = "fgsaf",
                Description = "safsafasfsa",
                Price = 122114,
                Count = 20
            };
            HttpResponseMessage response = await _client.PostAsJsonAsync($"/api/product", createProductDto);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task PostAsync_SendRequestWithAuthToken_ReturnsCreated()
        {
            var createProductDto = new CreateProductDTO()
            {
                Name = "fgsaf",
                Description = "safsafasfsa",
                Price = 122114,
                Count = 20
            };

            User user = new()
            {
                Email = "fsaf@fasf.fa",
                Name = "asfa",
                Id = 1,
                IsActive = true,
                IsEmailConfirmed = true,
                PasswordHash = "sfa",
                RoleId = 1,
                Role = new Role() { Name = "User" }
            };

            var token = _jwtProvider.GenerateToken(user.Email, user.Id.ToString(), user.Role.Name);

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.PostAsJsonAsync($"/api/product", createProductDto);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
        }
        [Test]
        public async Task PostAsync_SendRequestWithIncorrectAuthToken_ReturnsUnauthorized()
        {
            var createProductDto = new CreateProductDTO()
            {
                Name = "fgsaf",
                Description = "safsafasfsa",
                Price = 122114,
                Count = 20
            };

            User user = new()
            {
                Email = "fsaf@fasf.fa",
                Name = "asfa",
                Id = 1,
                IsActive = true,
                IsEmailConfirmed = true,
                PasswordHash = "sfa",
                RoleId = 1,
                Role = new Role() { Name = "User" }
            };

            var token = _jwtProvider.GenerateToken(user.Email, user.Id.ToString(), user.Role.Name);
            token = token + "s";

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.PostAsJsonAsync($"/api/product", createProductDto);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }
        [Test]
        public async Task DeleteAsync_SendRequestWithIncorrectClaimUserId_ReturnsForbidden()
        {
            int userId = 1;
            User user = new()
            {
                Email = "fsaf@fasf.fa",
                Name = "asfa",
                Id = userId,
                IsActive = true,
                IsEmailConfirmed = true,
                PasswordHash = "sfa",
                RoleId = 1,
                Role = new Role() { Name = "User" }
            };

            int productId = _products.First(p => p.OwnerId == userId + 1).Id;

            var token = _jwtProvider.GenerateToken(user.Email, user.Id.ToString(), user.Role.Name);

            _client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _client.DeleteAsync($"/api/product/{productId}");

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }
    }
}
