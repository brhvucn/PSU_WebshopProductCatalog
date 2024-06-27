using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Review.Application.Features.GetAggregatedByProduct;
using Webshop.Review.Application.Features.GetProductReviews;
using Webshop.Review.Application.Features.GetReview;

namespace Webshop.Review.Application.Test.Queries
{
    //test of GetProductReviewsQuery
    public class GetReviewQuery_Test
    {
        [Test]
        public void GetProductReviewsQuery_Constructor_Test()
        {
            //Arrange
            var reviewId = 1;
            //Act
            var query = new GetReviewQuery(reviewId);
            //Assert
            Assert.That(query.ReviewId, Is.EqualTo(reviewId));
        }

        [TestCase(-1)]
        [TestCase(0)]
        [Test]
        public void GetProductReviewsQuery_Constructor_InvalidProductId_Test(int id)
        {
            //Arrange
            var reviewId = id;
            //Act
            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new GetReviewQuery(reviewId));
        }

        [Test]
        public void GetProductReviewsQuery_Constructor_ValidProductId_Test()
        {
            //Arrange
            var reviewId = 1;
            //Act
            GetReviewQuery query = new GetReviewQuery(reviewId);
            //Assert
            Assert.That(query.ReviewId, Is.EqualTo(reviewId));

        }
    }
}
