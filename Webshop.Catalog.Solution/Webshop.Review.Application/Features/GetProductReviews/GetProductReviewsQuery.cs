using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Review.Domain;

namespace Webshop.Review.Application.Features.GetProductReviews
{
    public class GetProductReviewsQuery : IQuery<List<Domain.Review>>
    {
        public GetProductReviewsQuery(int productId)
        {
            Ensure.That(productId).IsGt(0);
            ProductId = productId;
        }
        public int ProductId { get; private set; }
    }
}
