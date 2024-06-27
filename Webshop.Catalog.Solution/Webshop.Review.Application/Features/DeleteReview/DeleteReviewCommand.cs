using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Review.Application.Features.DeleteReview
{
    public class DeleteReviewCommand : ICommand
    {
        public DeleteReviewCommand(int reviewId)
        {
            Ensure.That(reviewId, nameof(reviewId)).IsGte(1);
            ReviewId = reviewId;
        }
        public int ReviewId { get; private set; }
    }
}
