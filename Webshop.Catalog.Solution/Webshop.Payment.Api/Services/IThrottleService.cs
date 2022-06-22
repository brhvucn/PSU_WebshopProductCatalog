namespace PSU_PaymentGateway.Services
{
    public interface IThrottleService
    {
        bool CanExecute();
    }
}
