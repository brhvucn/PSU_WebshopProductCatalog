using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Review.Domain;

namespace Webshop.Review.Application.Features.GetReview
{
    public class GetReviewQuery : IQuery<Domain.Review>
    {
        public GetReviewQuery(int reviewId)
        {
            Ensure.That(reviewId).IsGt(0);
            ReviewId = reviewId;
        }
        public int ReviewId { get; private set; }
    }
}
