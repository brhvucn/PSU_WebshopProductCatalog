using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Webshop.ReviewServiceGateway.Controllers
{
    [Route("api/info")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok($"ReviewService Gateway");
        }
    }
}
