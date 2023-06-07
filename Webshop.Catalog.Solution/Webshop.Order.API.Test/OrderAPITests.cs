using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Webshop.Domain.AggregateRoots;
using Webshop.Order.Application.Features.Order.Dtos;
using Xunit;

namespace Webshop.Order.API.Test
{
    public class OrderAPITests
    {
        /// <summary>
        /// Expects to create an Order object and save it to the database
        /// </summary>
        /// <returns>Status Code</returns>
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
                    new { Product = new { ProductName = "Whispers of Eternity", SKU = "WOE-AB-01", Price = 1200, Currency = "EUR" }, Quantity = 1 },
                    new { Product = new { ProductName = "The Labyrinth's Key", SKU = "TLK-BM-01", Price = 500, Currency = "EUR" }, Quantity = 1  }
                }
            };

            // Act
            var response = await client.PostAsJsonAsync("api/orders", request);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Expects to retrieve all Order objects fromt the database
        /// </summary>
        /// <returns>Collection of OrderDto objects</returns>
        [Fact]
        public async Task GetAllOrders_ValidRequest_ReturnsIEnumerableOfOrderDto()
        {
            // Arrange
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:44363");
            var requestUrl = "api/order/getall";

            var expectedData = new[] {
                new {
                    Id = 1,
                    Customer = "Pavol Halás",
                    DateOfIssue = DateTime.Parse("2023-05-20").ToString(),
                    DueDate = DateTime.Parse("2023-06-10").ToString(),
                    Discount = 10,
                    OrderedProducts = new[] {
                        new { Product = new { ProductName = "Whispers of Eternity", SKU = "WOE-AB-01", Price = 1200, Currency = "EUR" }, Quantity = 1 },
                        new { Product = new { ProductName = "The Labyrinth's Key", SKU = "TLK-BM-01", Price = 500, Currency = "EUR" }, Quantity = 1 }
                },
                P = new {
                    Id = 2,
                    Customer = "Jožo Ráž",
                    DateOfIssue = DateTime.Parse("2023-05-20").ToString(),
                    DueDate = DateTime.Parse("2023-06-10").ToString(),
                    Discount = 10,
                    OrderedProducts = new[] {
                        new { Product = new { ProductName = "Shadows of Serendipity", SKU = "WOE-AB-01", Price = 1200, Currency = "EUR" }, Quantity = 1 },
                        new { Product = new { ProductName = "The Symphony of Secrets", SKU = "TLK-BM-01", Price = 500, Currency = "EUR" }, Quantity = 1 }
                        }
                    }
                }
            };
        
            // Act
            var response = await client.GetAsync(requestUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            var orders = await response.Content.ReadAsStringAsync();
            Assert.Contains(expectedData.ToString(), orders); // Assert that the order contains the expected data
        }

        /// <summary>
        /// Expects to retrieve an order based on the provided Id
        /// </summary>
        /// <returns>An OrderDto object of the corresponding Id</returns>
        [Fact]
        public async Task GetOrder_ValidRequest_ReturnsOrderDto()
        {
            // Arrange
            var orderId = 1;
            var requestUrl = $"api/order/{orderId}";

            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:44363");

            var expectedData = new
            {
                Id = 1,
                Customer = "Pavol Halás",
                DateOfIssue = DateTime.Parse("2023-05-20").ToString(),
                DueDate = DateTime.Parse("2023-06-10").ToString(),
                Discount = 10,
                OrderedProducts = new[] {
                    new { Product = new { ProductName = "Whispers of Eternity", SKU = "WOE-AB-01", Price = 1200, Currency = "EUR" }, Quantity = 1 },
                    new { Product = new { ProductName = "The Labyrinth's Key", SKU = "TLK-BM-01", Price = 500, Currency = "EUR" }, Quantity = 1  }
                }
            };

            // Act
            var response = await client.GetAsync(requestUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            var order = await response.Content.ReadAsStringAsync();

            Assert.NotNull(order); // Assert that the order is not null
            Assert.Contains(expectedData.ToString(), order); // Assert that the order contains the expected data
        }

        /// <summary>
        /// Expects to delete an order based on the provided Id
        /// </summary>
        /// <returns>Status Code</returns>
        [Fact]
        public async Task DeleteOrder_ValidRequest_ReturnsOk()
        {
            // Arrange
            var orderId = 1;
            var requestUrl = $"api/order/{orderId}";

            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:44363");

            // Act
            var response = await client.DeleteAsync(requestUrl);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}