using PSU_PaymentGateway.Models;
using PSU_PaymentGateway.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PSU_PaymentGatewayTest.Repository
{
    [Category("Repository Unit Tests")]     
    public class MemoryRepositoryUnitTest
    {
        [Fact]
        public void AddSingleTransactionExpectTrue()
        {
            //Arrange
            Result<Payment> paymentResult = Payment.Create("1234", "11/12", 123);
            Transaction transaction = Transaction.Create(1, paymentResult.Value).Value;
            IMemoryRepository memoryRepository = new MemoryRepository();
            //Act
            Result result = memoryRepository.AddTransaction(transaction);            
            //Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void AddMultipleTransactionExpectFalse()
        {
            //Arrange
            Result<Payment> paymentResult = Payment.Create("1234", "11/12", 123);
            Transaction transaction = Transaction.Create(1, paymentResult.Value).Value;
            IMemoryRepository memoryRepository = new MemoryRepository();
            //Act
            Result firstResult = memoryRepository.AddTransaction(transaction);
            Result secondResult = memoryRepository.AddTransaction(transaction);
            //Assert
            Assert.True(firstResult.IsSuccess);
            Assert.False(secondResult.IsSuccess);
        }
    }
}
