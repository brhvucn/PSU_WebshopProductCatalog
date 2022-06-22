using Microsoft.Extensions.Configuration;
using PSU_PaymentGateway.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Xunit;

namespace PSU_PaymentGatewayTest
{
    [Category("Service Tests")]
    public class ThrottleUnitTest
    {        
        private IConfiguration getConfiguration(int limit = 0)
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"Settings:Limit", limit.ToString()}
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            return config;
        }
        public ThrottleUnitTest()
        {
            //setup the config
           
        }

        [Fact]
        public void TestCanExecuteLimit0ExpectTrue()
        {
            //Arrange
            IThrottleService throttleService = new ThrottleService(getConfiguration(0));
            //Act
            bool canExecute = throttleService.CanExecute();
            //Assert
            Assert.True(canExecute);
        }

        [Fact]
        public void TestCanExecuteLimitNegativeExpectFalse()
        {
            //Arrange - first create an action to hold the constructor method call
            Action a = () => new ThrottleService(getConfiguration(-1));
            //Act - the constructor throws an exception
            Assert.Throws<ArgumentOutOfRangeException>(()=>a());
        }

        [Fact]
        public void TestCanInvokeTwiceWithoutThrottlingExpectTrue()
        {
            //Arrange
            IThrottleService throttleService = new ThrottleService(getConfiguration(0)); //disable throttling
            //Act
            bool canExecuteFirst = throttleService.CanExecute();
            bool canExecuteSecond = throttleService.CanExecute(); //second call, with no throttling, this should be true
            //Assert
            Assert.True(canExecuteFirst);
            Assert.True(canExecuteSecond);
        }

        [Fact]
        public void TestCanInvokeTwiceWithThrottlingExpectFalse()
        {
            //Arrange
            IThrottleService throttleService = new ThrottleService(getConfiguration(1000)); //enable throttling, for one second
            //Act
            bool canExecuteFirst = throttleService.CanExecute();
            bool canExecuteSecond = throttleService.CanExecute(); //second call, with no throttling, this should be true
            //Assert
            Assert.True(canExecuteFirst);
            Assert.False(canExecuteSecond);
        }
    }
}
