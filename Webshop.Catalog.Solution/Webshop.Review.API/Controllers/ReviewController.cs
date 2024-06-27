using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webshop.Application.Contracts;
using Webshop.Domain.Common;
using Webshop.Review.API.Models.Requests;
using Webshop.Review.API.Models.Validators;
using Webshop.Review.Application.Features.CreateReview;
using Webshop.Review.Application.Features.GetReview;

namespace Webshop.Review.API.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : BaseController
    {
        private readonly IDispatcher dispatcher;
        private readonly ILogger<ReviewController> logger;

        public ReviewController(IDispatcher dispatcher, ILogger<ReviewController> logger)
        {
            this.dispatcher = dispatcher;
            this.logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetReviews(int id)
        {
            try
            {
                GetReviewQuery query = new GetReviewQuery(id);
                var result = await this.dispatcher.Dispatch(query);
                return Ok();
            }
            catch(Exception ex)
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
    }
}
