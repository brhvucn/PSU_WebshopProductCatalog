using FluentValidation;
using Webshop.Review.API.Models.Requests;

namespace Webshop.Review.API.Models.Validators
{
    public class CreateReviewRequestValidator : AbstractValidator<CreateReviewRequest>
    {
        public CreateReviewRequestValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Rating).NotEmpty();
            RuleFor(x=>x.Rating).InclusiveBetween(1, 5); // rating must be between 1 and 5
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Comment).NotEmpty();
        }
    }
}
