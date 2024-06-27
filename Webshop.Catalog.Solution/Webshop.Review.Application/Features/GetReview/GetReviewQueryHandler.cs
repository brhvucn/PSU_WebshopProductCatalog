using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Review.Application.Contracts.Persistence;
using Webshop.Review.Application.Features.GetProductReviews;

namespace Webshop.Review.Application.Features.GetReview
{
    public class GetReviewQueryHandler : IQueryHandler<GetReviewQuery, Domain.Review>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<GetReviewQueryHandler> _logger;
        public GetReviewQueryHandler(ILogger<GetReviewQueryHandler> logger, IReviewRepository reviewRepository)
        {
            this._logger = logger;
            this._reviewRepository = reviewRepository;
        }

        public async Task<Result<Domain.Review>> Handle(GetReviewQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                return await this._reviewRepository.GetById(query.ReviewId);
            }
            catch(Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                return Result.Fail<Domain.Review>(Errors.General.FromException(ex));
            }
        }
    }
}
