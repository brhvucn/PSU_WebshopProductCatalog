using Webshop.Application.Contracts;

namespace Webshop.Order.Application.Features.Order.Commands.DeleteOrder
{
    public class DeleteOrderCommand : ICommand
    {
        public DeleteOrderCommand(int orderId)
        {
            OrderId = orderId;
        }

        public int OrderId { get; private set; }
    }
}
