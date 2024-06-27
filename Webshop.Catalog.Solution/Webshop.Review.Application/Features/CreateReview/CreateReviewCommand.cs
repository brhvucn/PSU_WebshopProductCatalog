using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Review.Application.Features.CreateReview
{
    public class CreateReviewCommand : ICommand
    {
        public CreateReviewCommand(int productId, int userId, string comment, int rating)
        {
            Ensure.That(productId).IsGt(0);
            Ensure.That(userId).IsGt(0);
            Ensure.That(comment).IsNotNullOrEmpty();
            Ensure.That(rating).IsGt(0);
            ProductId = productId;
            UserId = userId;
            Comment = comment;
            Rating = rating;
        }
        public int ProductId { get; private set; }
        public int UserId { get; private set; }
        public string Comment { get; private set; }
        public int Rating { get; private set; }
    }
}
