using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Review.Application.Features.GetAggregatedByProduct;

namespace Webshop.Review.Application.Test.Queries
{
    //test of GetAggretedByProductQuery
    public class GetAggregatedByProductQuery_Test
    {
        [Test]
        public void GetAggregatedByProductQuery_Constructor_Test()
        {
            //Arrange
            var productId = 1;
            //Act
            var query = new GetAggregatedByProductQuery(productId);
            //Assert
            Assert.That(query.ProductId, Is.EqualTo(query.ProductId));
        }

        [TestCase(-1)]
        [TestCase(0)]
        [Test]
        public void GetAggregatedByProductQuery_Constructor_InvalidProductId_Test(int id)
        {
            //Arrange
            var productId = id;
            //Act
            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new GetAggregatedByProductQuery(productId));
        }

        [Test]
        public void GetAggregatedByProductQuery_Constructor_ValidProductId_Test()
        {
            //Arrange
            var productId = 1;
            //Act
            GetAggregatedByProductQuery query = new GetAggregatedByProductQuery(productId);
            //Assert
            Assert.That(query.ProductId, Is.EqualTo(query.ProductId));
        }
    }
}
