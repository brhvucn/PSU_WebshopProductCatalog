using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Webshop.Order.API.Test
{
    public class OrderAPITests
    {
        [Fact]
        public async Task CreateOrder_ValidRequest_ReturnsOk()
        {
            // Arrange
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:44363");

            var request = new
            {
                Customer = "John Doe",
                DateOfIssue = "2023-05-20",
                DueDate = "2023-06-10",
                Discount = 10,
                OrderedProducts = new[] {
                    new { ProductId = 1, Quantity = 2 },
                    new { ProductId = 2, Quantity = 3 }
                }
            };

            // Act
            var response = await client.PostAsJsonAsync("api/orders", request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Order_ValidRequest_ReturnsOk()
        {
            // Arrange
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:44363");

            var request = new
            {
                Customer = "John Doe",
                DateOfIssue = "2023-05-20",
                DueDate = "2023-06-10",
                Discount = 10,
                OrderedProducts = new[] {
                    new { ProductId = 1, Quantity = 2 },
                    new { ProductId = 2, Quantity = 3 }
                }
            };

            // Act
            var response = await client.PostAsJsonAsync("api/orders", request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}