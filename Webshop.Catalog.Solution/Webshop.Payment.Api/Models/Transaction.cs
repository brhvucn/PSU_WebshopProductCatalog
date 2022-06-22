using EnsureThat;
using Microsoft.Extensions.Logging;
using System;

namespace PSU_PaymentGateway.Models
{
    public class Transaction
    {        
        private Transaction(int amount, Payment payment)
        {
            Amount = amount;
            Payment = payment;
            TransactionId = Guid.NewGuid();
            Created = DateTime.UtcNow;
        }
        static Transaction() { }
        public static Result<Transaction> Create(int amount, Payment payment)
        {
            try
            {
                Ensure.That(amount, nameof(amount)).IsGt<int>(0);
                Ensure.That(payment, nameof(payment)).IsNotNull<Payment>();                
                return Result.Ok<Transaction>(new Transaction(amount, payment));
            }
            catch (Exception ex)
            {
                return Result.Fail<Transaction>(ex.Message);
            }
        }
        public DateTime Created { get; private set; }
        public int Amount { get; private set; }
        public Payment Payment { get; private set; }
        public Guid TransactionId { get; private set; }
    }
}
