using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Review.Application.Features.GetAggregatedByProduct;
using Webshop.Review.Application.Features.GetProductReviews;

namespace Webshop.Review.Application.Test.Queries
{
    //test of GetProductReviewsQuery
    public class GetProductReviewsQuery_Test
    {
        [Test]
        public void GetProductReviewsQuery_Constructor_Test()
        {
            //Arrange
            var productId = 1;
            //Act
            var query = new GetProductReviewsQuery(productId);
            //Assert
            Assert.That(query.ProductId, Is.EqualTo(query.ProductId));
        }

        [TestCase(-1)]
        [TestCase(0)]
        [Test]
        public void GetProductReviewsQuery_Constructor_InvalidProductId_Test(int id)
        {
            //Arrange
            var productId = id;
            //Act
            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new GetProductReviewsQuery(productId));
        }

        [Test]
        public void GetProductReviewsQuery_Constructor_ValidProductId_Test()
        {
            //Arrange
            var productId = 1;
            //Act
            GetProductReviewsQuery query = new GetProductReviewsQuery(productId);
            //Assert
            Assert.That(query.ProductId, Is.EqualTo(query.ProductId));
        }
    }
}
