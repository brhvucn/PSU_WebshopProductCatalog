using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Webshop.Customer.Application.Contracts.Persistence;
using Webshop.Customer.Application.Features.CreateCustomer;
using Webshop.Customer.Application.Features.DeleteCustomer;
using Webshop.Customer.Application.Features.UpdateCustomer;
using Webshop.Domain.Common;
using Xunit;

namespace Webshop.Customer.Application.Test
{
    public class CustomerTests
    {
        [Fact]
        public void CreateCustomerCommand_InValid_ExpectFailure()
        {
            Action a = () => new CreateCustomerCommand(null);
            Assert.Throws<ArgumentNullException>(a);         
        }

        [Fact]
        public void UpdateCustomerCommand_InValid_ExpectFailure()
        {
            Action a = () => new UpdateCustomerCommand(null);
            Assert.Throws<ArgumentNullException>(a);
        }

        [Fact]
        public void DeleteCustomerCommand_InValid_ExpectFailure()
        {
            Action a = () => new DeleteCustomerCommand(0);
            Assert.Throws<ArgumentException>(a);
        }

        [Fact]
        public void CreateCustomerCommand_Valid_ExpectSuccess() 
        {
            //for the sake of completeness
            CreateCustomerCommand command = new CreateCustomerCommand(new Domain.AggregateRoots.Customer("Centisoft"));
        }

        [Fact]
        public async Task CreateCustomerCommandHandler_Invoke_repositorycreate_expect_success() 
        {
            //arrange
            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<CreateCustomerCommandHandler>>();            
            var customerRepositoryMock = new Mock<ICustomerRepository>();            
            Domain.AggregateRoots.Customer customer = new Domain.AggregateRoots.Customer("Centisoft");            
            CreateCustomerCommand command = new CreateCustomerCommand(customer);
            CreateCustomerCommandHandler handler = new CreateCustomerCommandHandler(loggerMock.Object, customerRepositoryMock.Object);
            //act
            Result result = await handler.Handle(command);
            //assert
            customerRepositoryMock.Verify((m => m.CreateAsync(customer)), Times.Once);            
            Assert.True(result.Success);
        }

        [Fact]
        public async Task UpdateCustomerCommandHandler_Invoke_repositoryupdate_expect_success()
        {
            //arrange
            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<UpdateCustomerCommandHandler>>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();
            Domain.AggregateRoots.Customer customer = new Domain.AggregateRoots.Customer("Centisoft") { Id = 1};
            UpdateCustomerCommand command = new UpdateCustomerCommand(customer);
            UpdateCustomerCommandHandler handler = new UpdateCustomerCommandHandler(loggerMock.Object, customerRepositoryMock.Object);
            //act
            Result result = await handler.Handle(command);
            //assert
            customerRepositoryMock.Verify((m => m.UpdateAsync(customer)), Times.Once);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task DeleteCustomerCommandHandler_Invoke_repositorydelete_expect_success()
        {
            //arrange
            var loggerMock = new Mock<Microsoft.Extensions.Logging.ILogger<DeleteCustomerCommandHandler>>();
            var customerRepositoryMock = new Mock<ICustomerRepository>();            
            DeleteCustomerCommand command = new DeleteCustomerCommand(1);
            DeleteCustomerCommandHandler handler = new DeleteCustomerCommandHandler(loggerMock.Object, customerRepositoryMock.Object);
            //act
            Result result = await handler.Handle(command);
            //assert
            customerRepositoryMock.Verify((m => m.DeleteAsync(1)), Times.Once);
            Assert.True(result.Success);
        }
    }
}
