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

namespace Webshop.Catalog.Application.Features.Product.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : ICommandHandler<DeleteProductCommand>
    {
        private ILogger<DeleteProductCommand> logger;
        private IProductRepository productRepository;
        public DeleteProductCommandHandler(ILogger<DeleteProductCommand> logger, IProductRepository productRepository)
        {
            this.logger = logger;
            this.productRepository = productRepository;
        }

        public async Task<Result> Handle(DeleteProductCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                await this.productRepository.DeleteAsync(command.ProductId);
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
