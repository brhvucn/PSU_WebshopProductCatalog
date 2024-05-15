using Webshop.Category.Application.Features.Category.Requests;

namespace Webshop.Catalog.Application.Test.RequestTests
{
    public class Tests
    {

        [Test]
        public void TestCreateCategoryRequest_InvalidNameIsNull_ExpectFailure()
        {
            //Arrange
            string name = null;
            string description = "my description";
            int parentId = 1;
            //create the request
            CreateCategoryRequest req = new CreateCategoryRequest();
            req.Name = name;
            req.Description = description;
            req.ParentId = parentId;
            //create the validator
            CreateCategoryRequest.Validator validator = new CreateCategoryRequest.Validator();
            //Act
            var validationResults = validator.Validate(req);
            //Assert, that this is the right error that was caught
            Assert.That(validationResults.Errors.Count, Is.EqualTo(1));
            Assert.That(validationResults.Errors[0].ErrorCode, Is.EqualTo("NotEmptyValidator"));
            Assert.That(validationResults.Errors[0].PropertyName, Is.EqualTo("Name"));
        }

        [Test]
        public void TestCreateCategoryRequest_InvalidNameIsEmpty_ExpectFailure()
        {
            //Arrange
            string name = "";
            string description = "my description";
            int parentId = 1;
            //create the request
            CreateCategoryRequest req = new CreateCategoryRequest();
            req.Name = name;
            req.Description = description;
            req.ParentId = parentId;
            //create the validator
            CreateCategoryRequest.Validator validator = new CreateCategoryRequest.Validator();
            //Act
            var validationResults = validator.Validate(req);
            //Assert, that this is the right error that was caught
            Assert.That(validationResults.Errors.Count, Is.EqualTo(1));
            Assert.That(validationResults.Errors[0].ErrorCode, Is.EqualTo("NotEmptyValidator"));
            Assert.That(validationResults.Errors[0].PropertyName, Is.EqualTo("Name"));
        }

        [Test]
        public void TestCreateCategoryRequest_InvalidParentIdNegative_ExpectFailure()
        {
            //Arrange
            string name = "valid name";
            string description = "my description";
            int parentId = -1;
            //create the request
            CreateCategoryRequest req = new CreateCategoryRequest();
            req.Name = name;
            req.Description = description;
            req.ParentId = parentId;
            //create the validator
            CreateCategoryRequest.Validator validator = new CreateCategoryRequest.Validator();
            //Act
            var validationResults = validator.Validate(req);
            //Assert, that this is the right error that was caught
            Assert.That(validationResults.Errors.Count, Is.EqualTo(1));
            Assert.That(validationResults.Errors[0].ErrorCode, Is.EqualTo("GreaterThanValidator"));
            Assert.That(validationResults.Errors[0].PropertyName, Is.EqualTo("ParentId"));
        }

        [Test]
        public void TestCreateCategoryRequest_InvalidParentIdZero_ExpectFailure()
        {
            //Arrange
            string name = "valid name";
            string description = "my description";
            int parentId = 0;
            //create the request
            CreateCategoryRequest req = new CreateCategoryRequest();
            req.Name = name;
            req.Description = description;
            req.ParentId = parentId;
            //create the validator
            CreateCategoryRequest.Validator validator = new CreateCategoryRequest.Validator();
            //Act
            var validationResults = validator.Validate(req);
            //Assert, that this is the right error that was caught
            Assert.That(validationResults.Errors.Count, Is.EqualTo(1));
            Assert.That(validationResults.Errors[0].ErrorCode, Is.EqualTo("GreaterThanValidator"));
            Assert.That(validationResults.Errors[0].PropertyName, Is.EqualTo("ParentId"));
        }
    }
}