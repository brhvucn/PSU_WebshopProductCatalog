using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Review.API.Models.Requests;
using Webshop.Review.API.Models.Validators;
using Webshop.Review.API.Utilities;
using Webshop.Review.Application.Features.CreateReview;
using Webshop.Review.Application.Features.DeleteReview;
using Webshop.Review.Application.Features.GetAggregatedByProduct;
using Webshop.Review.Application.Features.GetProductReviews;
using Webshop.Review.Application.Features.GetReview;
using Webshop.Review.Application.Features.GetReviews;
using Webshop.Review.Application.Features.GetUserReviews;
using Webshop.Review.Application.Features.UpdateReview;

namespace Webshop.Review.API.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : BaseController
    {
        private readonly IDispatcher dispatcher;
        private readonly ILogger<ReviewController> logger;
        private readonly InstanceHelper instanceHelper;

        public ReviewController(IDispatcher dispatcher, ILogger<ReviewController> logger, InstanceHelper instanceHelper)
        {
            this.dispatcher = dispatcher;
            this.logger = logger;
            this.instanceHelper = instanceHelper;
        }

        [HttpGet]
        [Route("/api/info")]
        public IActionResult Info()
        {
            return Ok($"Service: ReviewService");
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetReviews(int id)
        {
            try
            {
                //log to see load balancing in action
                this.logger.LogInformation($"Instance: {this.instanceHelper.GetInstanceId()}");
                //now process the query
                GetReviewQuery query = new GetReviewQuery(id);
                var result = await this.dispatcher.Dispatch(query);
                return FromResult(result);
            }
            catch(Exception ex)
            {
                return Error(Errors.General.FromException(ex));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            try
            {
                GetReviewsQuery query = new GetReviewsQuery();
                var result = await this.dispatcher.Dispatch(query);
                return FromResult(result);
            }
            catch (Exception ex)
            {
                return Error(Errors.General.FromException(ex));
            }
        }

        [HttpGet]
        [Route("products/{id}")]
        public async Task<IActionResult> GetReviewsForProduct(int id)
        {
            try
            {
                GetProductReviewsQuery query = new GetProductReviewsQuery(id);
                var result = await this.dispatcher.Dispatch(query);
                return FromResult(result);
            }
            catch (Exception ex)
            {
                return Error(Errors.General.FromException(ex));
            }
        }

        [HttpGet]
        [Route("productsaggregated/{id}")]
        public async Task<IActionResult> GetReviewsForProductAggregated(int id)
        {
            try
            {
                GetAggregatedByProductQuery query = new GetAggregatedByProductQuery(id);
                var result = await this.dispatcher.Dispatch(query);
                return FromResult(result);
            }
            catch (Exception ex)
            {
                return Error(Errors.General.FromException(ex));
            }
        }

        [HttpGet]
        [Route("users/{id}")]
        public async Task<IActionResult> GetReviewsForUser(int id)
        {
            try
            {
                GetUserReviewsQuery query = new GetUserReviewsQuery(id);
                var result = await this.dispatcher.Dispatch(query);
                return FromResult(result);
            }
            catch (Exception ex)
            {
                return Error(Errors.General.FromException(ex));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody]CreateReviewRequest request)
        {
            try
            {
                CreateReviewRequestValidator validator = new CreateReviewRequestValidator();
                var validationResult = validator.Validate(request);
                if(!validationResult.IsValid)
                {
                    return Error("");
                    //return Error(Errors.Validation.FromValidationRules(validationResult.Errors));
                }
                //create and dispatch the command
                CreateReviewCommand command = new CreateReviewCommand(request.ProductId, request.UserId, request.Comment, request.Rating);
                var commandResult = await this.dispatcher.Dispatch(command);
                return FromResult(commandResult);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateReview([FromBody] UpdateReviewRequest request)
        {
            try
            {
                UpdateReviewRequestValidator validator = new UpdateReviewRequestValidator();
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    return Error("");
                    //return Error(Errors.Validation.FromValidationRules(validationResult.Errors));
                }
                //create and dispatch the command
                UpdateReviewCommand command = new UpdateReviewCommand(request.ReviewId, request.Comment, request.Rating);
                var commandResult = await this.dispatcher.Dispatch(command);
                return FromResult(commandResult);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteReview([FromBody] DeleteReviewRequest request)
        {
            try
            {
                DeleteReviewValidator validator = new DeleteReviewValidator();
                var validationResult = validator.Validate(request);
                if (!validationResult.IsValid)
                {
                    //return Error("");
                    return Error(Errors.Validation.FromValidationRules(validationResult.Errors));
                }
                //create and dispatch the command
                DeleteReviewCommand command = new DeleteReviewCommand(request.ReviewId);
                var commandResult = await this.dispatcher.Dispatch(command);
                return FromResult(commandResult);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
