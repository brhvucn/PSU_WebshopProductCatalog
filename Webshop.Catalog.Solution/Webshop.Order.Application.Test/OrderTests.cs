using Moq;
using System;
using System.Collections.Generic;
using Webshop.Domain.AggregateRoots;
using Webshop.Order.Application.Features.Order.Commands.CreateOrder;
using Xunit;

namespace Webshop.Order.Application.Test
{
    public class OrderTests
    {
        [Fact]
        public void CreateOrderCommand_InValid_ExpectFailure()
        {
            Dictionary<Catalog.Domain.AggregateRoots.Product, int> orderedProducts = null;
            Action a = () => new CreateOrderCommand(null, DateTime.MinValue, DateTime.MinValue, 0, orderedProducts);
            Assert.Throws<ArgumentNullException>(a);
        }
    }
}
