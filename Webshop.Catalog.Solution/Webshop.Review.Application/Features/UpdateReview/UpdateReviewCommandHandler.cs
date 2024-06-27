using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Review.Application.Contracts.Persistence;
using Webshop.Review.Application.Features.DeleteReview;

namespace Webshop.Review.Application.Features.UpdateReview
{
    public class UpdateProductReviewCommandHandler : ICommandHandler<UpdateReviewCommand>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<UpdateProductReviewCommandHandler> _logger;
        public UpdateProductReviewCommandHandler(ILogger<UpdateProductReviewCommandHandler> logger, IReviewRepository reviewRepository)
        {
            this._logger = logger;
            this._reviewRepository = reviewRepository;
        }

        public async Task<Result> Handle(UpdateReviewCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                //get from repository                
                Domain.Review review = await this._reviewRepository.GetById(command.ReviewId);
                if(review == null)
                {
                    return Result.Fail(Errors.General.NotFound<int>(command.ReviewId));
                }
                //set the new values
                review.SetRatingComment(command.Comment, command.Rating); // to avoid opening the setters to public and to be able to encapsulate the validation logic
                //update the review
                await this._reviewRepository.UpdateAsync(review);
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
