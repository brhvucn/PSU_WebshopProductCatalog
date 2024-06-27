using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Review.Application.Features.GetUserReviews
{
    public class GetUserReviewsQuery : IQuery<List<Domain.Review>>
    {
        public GetUserReviewsQuery(int userId)
        {
            Ensure.That(userId).IsGt(0);
            UserId = userId;
        }
        public int UserId { get; private set; }
    }
}
