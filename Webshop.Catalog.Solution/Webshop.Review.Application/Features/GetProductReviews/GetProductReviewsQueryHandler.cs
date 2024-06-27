using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Review.Application.Contracts.Persistence;
using Webshop.Review.Application.Features.GetAggregatedByProduct;

namespace Webshop.Review.Application.Features.GetProductReviews
{
    public class GetProductReviewsQueryHandler : IQueryHandler<GetProductReviewsQuery, List<Domain.Review>>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<GetProductReviewsQueryHandler> _logger;
        public GetProductReviewsQueryHandler(ILogger<GetProductReviewsQueryHandler> logger, IReviewRepository reviewRepository)
        {
            this._logger = logger;
            this._reviewRepository = reviewRepository;
        }

        public async Task<Result<List<Domain.Review>>> Handle(GetProductReviewsQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                return await this._reviewRepository.GetByProduct(query.ProductId);
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                return Result.Fail<List<Domain.Review>>(Errors.General.FromException(ex));
            }
        }
    }
}
