using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Review.Application.Contracts.Persistence;

namespace Webshop.Review.Application.Features.DeleteReview
{
    public class DeleteReviewCommandHandler : ICommandHandler<DeleteReviewCommand>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<DeleteReviewCommandHandler> _logger;
        public DeleteReviewCommandHandler(ILogger<DeleteReviewCommandHandler> logger, IReviewRepository reviewRepository)
        {
            this._logger = logger;
            this._reviewRepository = reviewRepository;
        }
        public async Task<Result> Handle(DeleteReviewCommand command, CancellationToken cancellationToken = default)
        {
            try
            {                
                await this._reviewRepository.DeleteAsync(command.ReviewId);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.General.FromException(ex));
            }
        }
    }
}
