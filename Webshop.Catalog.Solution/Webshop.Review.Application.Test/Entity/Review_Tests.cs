using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webshop.Review.Application.Test.Entity
{
    public class Review_Tests
    {
        [Test]
        public void Constructor_WithValidParameters_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            int productId = 1;
            int userId = 1;
            string comment = "Excellent product!";
            int rating = 5;

            // Act
            var review = new Domain.Review(productId, userId, comment, rating);

            // Assert
            Assert.That(review.ProductId, Is.EqualTo(productId));
            Assert.That(review.UserId, Is.EqualTo(userId));
            Assert.That(review.Comment, Is.EqualTo(comment));
            Assert.That(review.Rating, Is.EqualTo(rating));
        }

        [TestCase(-1, 1, "Great!", 4)]
        [TestCase(1, -1, "Great!", 4)]        
        [TestCase(1, 1, "Good", 0)]
        [TestCase(1, 1, "Good", 6)]
        public void Constructor_WithInvalidParameters_ShouldThrowArgumentException(int productId, int userId, string comment, int rating)
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new Domain.Review(productId, userId, comment, rating));
        }

        [TestCase(1, 1, null, 4)]
        public void Constructor_WithInvalidParameters_ShouldThrowArgumentNullException(int productId, int userId, string comment, int rating)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new Domain.Review(productId, userId, comment, rating));
        }

        [Test]
        public void SetRatingComment_WithValidParameters_ShouldUpdateProperties()
        {
            // Arrange
            var review = new Domain.Review(1, 1, "Good", 4);
            string newComment = "Updated comment";
            int newRating = 3;

            // Act
            review.SetRatingComment(newComment, newRating);

            // Assert
            Assert.That(review.Comment, Is.EqualTo(newComment));
            Assert.That(review.Rating, Is.EqualTo(newRating));
        }

        [TestCase(null, 5)]
        public void SetRatingComment_WithInvalidParameters_ShouldThrowArgumentNullException(string comment, int rating)
        {
            // Arrange
            var review = new Domain.Review(1, 1, "Good", 4);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => review.SetRatingComment(comment, rating));
        }

        [TestCase("Updated comment", 0)]
        [TestCase("Updated comment", 6)]   
        public void SetRatingComment_WithInvalidParameters_ShouldThrowArgumentRangeException(string comment, int rating)
        {
            // Arrange
            var review = new Domain.Review(1, 1, "Good", 4);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => review.SetRatingComment(comment, rating));
        }

        [TestCase("", 5)]
        public void SetRatingComment_WithInvalidParameters_ShouldThrowArgumentException(string comment, int rating)
        {
            // Arrange
            var review = new Domain.Review(1, 1, "Good", 4);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => review.SetRatingComment(comment, rating));
        }
    }
}
