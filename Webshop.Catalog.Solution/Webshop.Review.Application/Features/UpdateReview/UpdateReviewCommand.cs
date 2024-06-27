using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Review.Application.Features.UpdateReview
{
    public class UpdateReviewCommand : ICommand
    {
        public UpdateReviewCommand(int reviewId, string comment, int rating)
        {
            Ensure.That(comment).IsNotNullOrEmpty();
            Ensure.That(rating).IsGt(0);
            Ensure.That(reviewId).IsGt(0);            
            Comment = comment;
            Rating = rating;
            ReviewId = reviewId;
        }
        public int ReviewId { get; private set; }
        public string Comment { get; private set; }
        public int Rating { get; private set; }
    }
}
