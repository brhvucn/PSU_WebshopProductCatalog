using Webshop.Review.Application.Features.CreateReview;

namespace Webshop.Review.Application.Test.Commands
{
    public class CreateCommand_Test
    {

        [Test]
        public void Constructor_WithValidParameters_ShouldInitializeProperties()
        {
            // Arrange
            int productId = 1;
            int userId = 1;
            string comment = "Great product!";
            int rating = 5;

            // Act
            var command = new CreateReviewCommand(productId, userId, comment, rating);

            // Assert
            Assert.That(command.ProductId, Is.EqualTo(command.ProductId));
            Assert.That(command.UserId, Is.EqualTo(command.UserId));
            Assert.That(command.Comment, Is.EqualTo(command.Comment));
            Assert.That(command.Rating, Is.EqualTo(command.Rating));
        }

        [TestCase(1, 1, "", 3)]        
        public void Constructor_WithInvalidParameters_ShouldThrowArgumentException(int productId, int userId, string comment, int rating)
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new CreateReviewCommand(productId, userId, comment, rating));
        }
        
        [TestCase(1, 1, null, 3)]
        public void Constructor_WithInvalidParameters_ShouldThrowArgumentNullException(int productId, int userId, string comment, int rating)
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CreateReviewCommand(productId, userId, comment, rating));
        }

        [TestCase(-1, 1, "Nice!", 3)]
        [TestCase(1, -1, "Nice!", 3)]
        [TestCase(1, 1, "Nice!", 0)]
        public void Constructor_WithInvalidParameters_ShouldThrowArgumentOutOfRangeException(int productId, int userId, string comment, int rating)
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => new CreateReviewCommand(productId, userId, comment, rating));
        }

        [Test]
        public void Constructor_WithNullComment_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => new CreateReviewCommand(1, 1, null, 4));
        }
    }
}