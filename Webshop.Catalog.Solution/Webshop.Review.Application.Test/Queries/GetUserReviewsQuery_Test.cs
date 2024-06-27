using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Review.Application.Features.GetAggregatedByProduct;
using Webshop.Review.Application.Features.GetProductReviews;
using Webshop.Review.Application.Features.GetReview;
using Webshop.Review.Application.Features.GetUserReviews;

namespace Webshop.Review.Application.Test.Queries
{
    //test of GetProductReviewsQuery
    public class GetUserReviewsQuery_Test
    {
        [Test]
        public void GetUserReviewsQuery_Constructor_Test()
        {
            //Arrange
            var reviewId = 1;
            //Act
            var query = new GetUserReviewsQuery(reviewId);
            //Assert
            Assert.That(query.UserId, Is.EqualTo(query.UserId));
        }

        [TestCase(-1)]
        [TestCase(0)]
        [Test]
        public void GetUserReviewsQuery_Constructor_InvalidProductId_Test(int id)
        {
            //Arrange
            var reviewId = id;
            //Act
            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new GetUserReviewsQuery(reviewId));
        }

        [Test]
        public void GetUserReviewsQuery_Constructor_ValidProductId_Test()
        {
            //Arrange
            var userId = 1;
            //Act
            GetUserReviewsQuery query = new GetUserReviewsQuery(userId);
            //Assert
            Assert.That(query.UserId, Is.EqualTo(query.UserId));
        }
    }
}
