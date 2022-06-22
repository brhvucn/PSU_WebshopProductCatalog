using EnsureThat;
using Microsoft.Extensions.Configuration;
using System;

namespace PSU_PaymentGateway.Services
{
    public class ThrottleService : IThrottleService
    {
        private int Limit = 0; //the limit/duration between requests
        private long lastRequestms = 0L; //the last request
        public ThrottleService(IConfiguration configuration)
        {
            int tmpLimit = configuration.GetValue<int>("Settings:Limit");
            Ensure.That(tmpLimit, nameof(tmpLimit)).IsGte<int>(0);
            this.Limit = configuration.GetValue<int>("Settings:Limit");
        }
        public bool CanExecute()
        {
            if (Limit > 0 && lastRequestms > 0)
            {
                //1 tick is 1/10.000 ms
                long ms = DateTime.Now.Ticks / 10000L;
                long diff = ms - lastRequestms;
                this.lastRequestms = ms / 10000L;
                return diff > Limit;
            }
            else
            {
                //set the last request
                this.lastRequestms = DateTime.Now.Ticks / 10000L;                                
                //not throttled
                return true;
            }
        }
    }
}
