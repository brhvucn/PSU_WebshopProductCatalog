using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Review.Application.Contracts.Persistence;
using Webshop.Review.Application.Features.GetReview;

namespace Webshop.Review.Application.Features.GetReviews
{
    public class GetReviewsQueryHandler : IQueryHandler<GetReviewsQuery, List<Domain.Review>>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<GetReviewsQueryHandler> _logger;

        public GetReviewsQueryHandler(ILogger<GetReviewsQueryHandler> logger, IReviewRepository reviewRepository)
        {
            this._reviewRepository = reviewRepository;
            this._logger = logger;
        }

        public async Task<Result<List<Domain.Review>>> Handle(GetReviewsQuery query, CancellationToken cancellationToken = default)
        {
            var tmpList = await this._reviewRepository.GetAll();
            return tmpList.ToList();
        }
    }
}
