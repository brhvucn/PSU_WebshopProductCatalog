using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Contracts.Persistence;
using Webshop.Domain.Common;

namespace Webshop.Catalog.Application.Features.Product.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
    {
        private ILogger<UpdateProductCommand> logger;
        private IProductRepository productReopsitory;
        public UpdateProductCommandHandler(ILogger<UpdateProductCommand> logger, IProductRepository productReopsitory)
        {
            this.logger = logger;
            this.productReopsitory = productReopsitory;
        }

        public async Task<Result> Handle(UpdateProductCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                Domain.AggregateRoots.Product product = new Domain.AggregateRoots.Product(command.Name, command.SKU, command.Price, command.Currency);
                product.AmountInStock = command.AmountInStock;
                product.MinStock = command.MinStock;
                product.Description = command.Description;
                product.Id = command.Id;
                await this.productReopsitory.UpdateAsync(product);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
