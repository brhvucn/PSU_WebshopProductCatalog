using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Webshop.Application.Contracts;
using Webshop.Domain.AggregateRoots;
using Webshop.Domain.Common;
using Webshop.Order.Api.Controllers;
using Webshop.Order.Application.Features.Order.Requests;

namespace Webshop.Order.Api.Tests
{
    public class OrderApiTests
    {
        [Fact]
        public async Task CreateOrder_withProperInput_shouldWorkProperly()
        {
            // Arrange
            var mockDispatcher = new Mock<IDispatcher>();
            mockDispatcher
                .Setup(m => m.Dispatch(It.IsAny<ICommand>()))
                .ReturnsAsync(Result.Ok());

            var mockMapper = new Mock<IMapper>();
            var mockLogger = new Mock<ILogger<OrderController>>();

            var orderController = new OrderController(mockDispatcher.Object, mockMapper.Object, mockLogger.Object);

            var orderedProducts = new Dictionary<Catalog.Domain.AggregateRoots.Product, int>()
            {
                {new Catalog.Domain.AggregateRoots.Product("All quiet on the Western Front", "LB-EMR-01", 1500, "EUR"), 1}
            };

            // Act
            var actionResult = await orderController.CreateOrder(new CreateOrderRequest
            {
                Customer = new Customer("Samo"),
                DateOfIssue = DateTime.Today,
                DueDate = DateTime.Today.AddDays(21),
                Discount = 0,
                OrderedProducts = orderedProducts
            });

            // Assert
            Assert.IsType<OkObjectResult>(actionResult);
        }
    }
}