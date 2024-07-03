using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webshop.Domain.Common;
using Webshop.Review.Application.Features.GetProductReviews;

namespace Webshop.Review.API.Controllers
{
    [Route("api/demoreview")]
    [ApiController]
    public class DemoReviewController : BaseController
    {
        [HttpGet]
        [Route("products/{id}")]
        public async Task<IActionResult> GetReviewsForProduct(int id)
        {
            try
            {
                Domain.Review review1 = new Domain.Review(id, 2, "Nice Paddleboard", 4);
                Domain.Review review2 = new Domain.Review(id, 3, "Very good Paddleboard", 2);
                Domain.Review review3 = new Domain.Review(id, 4, "I like this Paddleboard", 3);
                Domain.Review review4 = new Domain.Review(id, 5, "Wow! thats the best Paddleboard", 5);
                GetProductReviewsQuery query = new GetProductReviewsQuery(id);
                var result = new List<Domain.Review>
                {
                    review1,
                    review2,
                    review3,
                    review4
                };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Error(Errors.General.FromException(ex));
            }
        }
    }
}
