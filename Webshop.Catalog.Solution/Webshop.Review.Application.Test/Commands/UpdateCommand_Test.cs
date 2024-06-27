using Webshop.Review.Application.Features.UpdateReview;

namespace Webshop.Review.Application.Test.Commands
{
    public class UpdateCommand_Test
    {

        [Test]
        public void Constructor_WithValidParameters_ShouldInitializeProperties()
        {
            // Arrange
            int reviewId = 1;
            string comment = "Great product!";
            int rating = 5;

            // Act
            var command = new UpdateReviewCommand(reviewId, comment, rating);

            // Assert
            Assert.That(command.ReviewId, Is.EqualTo(reviewId));
            Assert.That(command.Comment, Is.EqualTo(comment));
            Assert.That(command.Rating, Is.EqualTo(rating));
        }

        [TestCase(1, "", 3)]        
        public void Constructor_WithInvalidParameters_ShouldThrowArgumentException(int reviewId, string comment, int rating)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new UpdateReviewCommand(reviewId, comment, rating));
        }
        
        [TestCase(1, null, 3)]
        public void Constructor_WithInvalidParameters_ShouldThrowArgumentNullException(int reviewId, string comment, int rating)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new UpdateReviewCommand(reviewId, comment, rating));
        }

        [TestCase(-1, "Nice!", 3)]
        [TestCase(1, "Nice!", 6)]
        [TestCase(1, "Nice!", -1)]
        public void Constructor_WithInvalidParameters_ShouldThrowArgumentOutOfRangeException(int reviewId, string comment, int rating)
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new UpdateReviewCommand(reviewId, comment, rating));
        }

        [Test]
        public void Constructor_WithNullComment_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new UpdateReviewCommand(1, null, 4));
        }
    }
}