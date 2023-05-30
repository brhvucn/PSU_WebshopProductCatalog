using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Webshop.Order.Api.Utilities;
using Webshop.Domain.Common;
using Webshop.Domain.ValueObjects;

namespace Webshop.Order.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected new ActionResult Ok()
        {
            return base.Ok(Envelope.Ok());
        }

        protected ActionResult Ok<T>(T result)
        {
            return base.Ok(Envelope.Ok(result));
        }

        protected ActionResult Error(List<string> errorMessages)
        {
            string errors = string.Join(";", errorMessages);
            return BadRequest(Envelope.Error(errors));
        }

        protected ActionResult Error(string errorMessage)
        {
            return BadRequest(Envelope.Error(errorMessage));
        }

        protected ActionResult Error(Error error)
        {
            return BadRequest(Envelope.Error(error.Message + " (" + error.Code + ")"));
        }

        protected ActionResult Error(List<ValidationFailure> validationErrors)
        {
            List<string> errors = validationErrors.Select(x => x.ErrorMessage + " (" + x.PropertyName + ")").ToList();
            return Error(errors);
        }

        protected IActionResult FromResult(Result result)
        {
            if (result.Failure)
                return StatusCodeFromResult(result);
            return base.Ok(Envelope.Ok());
        }

        protected IActionResult FromResult<T>(Result<T> result)
        {
            if (result.Failure)
                return StatusCodeFromResult(result);

            return base.Ok(Envelope.Ok(result.Value));
        }

        private IActionResult StatusCodeFromResult(Result result)
           => StatusCode(result.Error.StatusCode, Envelope.Error(result.Error.Code));
    }
}
