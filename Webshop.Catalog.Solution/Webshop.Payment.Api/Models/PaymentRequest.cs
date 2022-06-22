using EnsureThat;

namespace PSU_PaymentGateway.Models
{
    /// <summary>
    /// This is the entity payment
    /// </summary>
    public class PaymentRequest
    {
        public PaymentRequest(int amount, string cardnumber, string expirationDate, int cvc)
        {
            Ensure.That(amount, nameof(amount)).IsGt<int>(0);
            Ensure.That(cardnumber, nameof(cardnumber)).IsNotNullOrEmpty();
            Ensure.That(expirationDate, nameof(expirationDate)).IsNotNullOrEmpty();
            Ensure.That(cvc, nameof(cvc)).IsGte(100);
            Ensure.That(cvc, nameof(cvc)).IsLt(1000);
            //set the properties
            CardNumber = cardnumber;
            ExpirationDate = expirationDate;
            CVC = cvc;
            Amount = amount;
        }
        public string CardNumber { get; private set; }
        public string ExpirationDate { get; private set; }
        public int CVC { get; private set; }
        public int Amount { get; private set; }
    }
}
