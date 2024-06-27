using FluentValidation;
using Webshop.Review.API.Models.Requests;

namespace Webshop.Review.API.Models.Validators
{
    public class DeleteReviewValidator : AbstractValidator<DeleteReviewRequest>
    {
        public DeleteReviewValidator()
        {
            RuleFor(x => x.ReviewId).NotEmpty();
        }
    }
}
