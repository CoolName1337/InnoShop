using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductService.Contracts.DTOs;
using ProductService.Contracts.Interfaces;
using System.Diagnostics;
using System.Net;

namespace ProductService.API.IntegrationTests
{
    [TestFixture]
    public class ProductControllerTests
    {
        private HttpClient client;
        private WebApplicationFactory<Program> webHost;
        [SetUp]
        public async Task Setup()
        {
            webHost = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    var serviceDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IProductService));
                    services.Remove(serviceDescriptor);

                    var productServiceMock = new Mock<IProductService>();

                    productServiceMock
                        .Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
                        .Returns(Task.FromResult(new List<ProductDTO>()
                        {
                            new (),
                            new (),
                            new ()
                        }));
                    services.AddTransient(_ => productServiceMock.Object);
                });
            });
            client = webHost.CreateClient();
        }

        [Test]
        public async Task GetAllAsync_SendRequest_ReturnsOk()
        {
            HttpResponseMessage response = await client.GetAsync("/api/product");

            Debug.WriteLine(await response.Content.ReadAsStringAsync());

            Assert.That(response.StatusCode == HttpStatusCode.OK);
        }
    }
}
