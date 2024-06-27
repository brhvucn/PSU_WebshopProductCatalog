using EnsureThat;
using Webshop.Domain.Common;

namespace Webshop.Review.Domain
{
    public class Review : AggregateRoot
    {
        public Review() { } //for Dapper only
        public Review(int productId, int userId, string comment, int rating)
        {
            Ensure.That(productId).IsGt(0);
            Ensure.That(userId).IsGt(0);
            Ensure.That(comment).IsNotNullOrEmpty();
            Ensure.That(rating).IsGt(0);
            Ensure.That(rating).IsLte(5); // rating must be between 1 and 5

            ProductId = productId;
            UserId = userId;
            Comment = comment;
            Rating = rating;
        }
        public int ProductId { get; private set; }
        public int UserId { get; private set; }
        public string Comment { get; private set; } = string.Empty;
        public int Rating { get; private set; }

        public void SetRatingComment(string comment, int rating)
        {
            Ensure.That(comment).IsNotNullOrEmpty();
            Ensure.That(rating).IsGt(0);
            Ensure.That(rating).IsLte(5); // rating must be between 1 and 5
            Comment = comment;
            Rating = rating;
        }
    }
}
