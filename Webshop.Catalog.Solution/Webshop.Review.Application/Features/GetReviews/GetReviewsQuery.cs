using EnsureThat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;

namespace Webshop.Review.Application.Features.GetReviews
{
    //Empty since we don't need any parameters
    public class GetReviewsQuery : IQuery<List<Domain.Review>>
    {        
        public GetReviewsQuery()
        {
        }        
    }
}
