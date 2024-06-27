using FluentValidation.TestHelper;
using Webshop.Review.API.Models.Requests;
using Webshop.Review.API.Models.Validators;

namespace Webshop.Review.API.Tests
{
    [TestFixture]
    public class RequestsTests
    {
        private CreateReviewRequestValidator validator;

        [SetUp]
        public void Setup()
        {
            validator = new CreateReviewRequestValidator();
        }

        [Test]
        public void Should_have_error_when_ProductId_is_zero()
        {
            var model = new CreateReviewRequest { ProductId = 0 };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(r => r.ProductId);
        }

        [Test]
        public void Should_not_have_error_when_ProductId_is_greater_than_zero()
        {
            var model = new CreateReviewRequest { ProductId = 1 };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(r => r.ProductId);
        }

        [Test]
        public void Should_have_error_when_Rating_is_out_of_range()
        {
            var model = new CreateReviewRequest { Rating = 6 };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(r => r.Rating);
        }

        [Test]
        public void Should_not_have_error_when_Rating_is_within_range()
        {
            var model = new CreateReviewRequest { Rating = 5 };
            var result = validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(r => r.Rating);
        }

        [Test]
        public void Should_have_error_when_UserId_is_zero()
        {
            var model = new CreateReviewRequest { UserId = 0 };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(r => r.UserId);
        }

        [Test]
        public void Should_have_error_when_Comment_is_empty()
        {
            var model = new CreateReviewRequest { Comment = "" };
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(r => r.Comment);
        }
    }
}