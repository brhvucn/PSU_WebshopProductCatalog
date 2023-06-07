using EnsureThat;
using Webshop.Application.Contracts;

namespace Webshop.Order.Application.Features.Order.Commands.DeleteOrder
{
    public class DeleteOrderCommand : ICommand
    {
        public DeleteOrderCommand(int orderId)
        {
            //Smallest possible id is 1
            Ensure.That(orderId, nameof(orderId)).IsGt(0);
            OrderId = orderId;
        }

        public int OrderId { get; private set; }
    }
}
