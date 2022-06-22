using EnsureThat;
using System;

namespace PSU_PaymentGateway.Models
{
    /// <summary>
    /// This is the entity payment
    /// </summary>
    public class Payment
    {
        public static Result<Payment> Create(string cardnumber, string expirationDate, int cvc)
        {
            try
            {
                return Result.Ok<Payment>(new Payment(cardnumber, expirationDate, cvc));
            }
            catch (Exception e)
            {
                return Result.Fail<Payment>(e.Message);
            }
        }

        private Payment(string cardnumber, string expirationDate, int cvc)
        {
            Ensure.That(cardnumber, nameof(cardnumber)).IsNotNullOrEmpty();
            Ensure.That(expirationDate, nameof(expirationDate)).IsNotNullOrEmpty();
            Ensure.That(expirationDate, nameof(expirationDate)).Matches(@"[0-9]{2}\/[0-9]{2}"); //regex matches 05/12 etc
            Ensure.That(cvc, nameof(cvc)).IsGte(100);
            Ensure.That(cvc, nameof(cvc)).IsLt(1000);
            //set the properties
            CardNumber = cardnumber;
            ExpirationDate = expirationDate;
            CVC = cvc;
        }

        public string CardNumber { get; private set; }
        public string ExpirationDate { get; private set; }
        public int CVC { get; private set; }
    }
}
