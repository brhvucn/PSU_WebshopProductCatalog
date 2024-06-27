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

namespace Webshop.Review.Application.Features.GetUserReviews
{
    public class GetUserReviewsQueryHandler : IQueryHandler<GetUserReviewsQuery, List<Domain.Review>>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<GetUserReviewsQueryHandler> _logger;
        public GetUserReviewsQueryHandler(ILogger<GetUserReviewsQueryHandler> logger, IReviewRepository reviewRepository)
        {
            this._logger = logger;
            this._reviewRepository = reviewRepository;
        }

        public async Task<Result<List<Domain.Review>>> Handle(GetUserReviewsQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                return await this._reviewRepository.GetByUser(query.UserId);
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                return Result.Fail<List<Domain.Review>>(Errors.General.FromException(ex));
            }
        }
    }
}
