using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Review.Application.Contracts.Persistence;
using Webshop.Review.Domain;

namespace Webshop.Review.Application.Features.CreateReview
{
    public class CreateReviewCommandHandler : ICommandHandler<CreateReviewCommand>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<CreateReviewCommandHandler> _logger;
        public CreateReviewCommandHandler(ILogger<CreateReviewCommandHandler> logger, IReviewRepository reviewRepository)
        {
            this._reviewRepository = reviewRepository;
            this._logger = logger;
        }
        public async Task<Result> Handle(CreateReviewCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                Domain.Review review = new Domain.Review(command.ProductId, command.UserId, command.Comment, command.Rating);
                await this._reviewRepository.CreateAsync(review);
                return Result.Ok();
            }
            catch(Exception ex)
            {
                this._logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.General.FromException(ex));
            }
        }
    }
}
