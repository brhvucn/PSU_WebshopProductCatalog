using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Review.Application.Contracts.Persistence;

namespace Webshop.Review.Application.Features.GetAggregatedByProduct
{
    public class GetAggregatedByProductQueryHandler : IQueryHandler<GetAggregatedByProductQuery, ProductReviewDTO>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<GetAggregatedByProductQueryHandler> _logger;        
        public GetAggregatedByProductQueryHandler(ILogger<GetAggregatedByProductQueryHandler> logger, IReviewRepository reviewRepository)
        {
            this._logger = logger;
            this._reviewRepository = reviewRepository;
        }

        public async Task<Result<ProductReviewDTO>> Handle(GetAggregatedByProductQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                return await this._reviewRepository.GetAggregatedByProduct(query.ProductId);
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                return Result.Fail<ProductReviewDTO>(Errors.General.FromException(ex));
            }
        }
    }
}
