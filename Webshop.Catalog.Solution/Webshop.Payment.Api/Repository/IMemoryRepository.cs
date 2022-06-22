
using PSU_PaymentGateway.Models;

namespace PSU_PaymentGateway.Repository
{
    public interface IMemoryRepository
    {
        Result AddTransaction(Transaction transaction);
    }
}
