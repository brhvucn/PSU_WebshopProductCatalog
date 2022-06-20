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

namespace Webshop.Catalog.Application.Features.Product.Commands.AttachProductToCategory
{
    public class AttachProductToCategoryCommandHandler : ICommandHandler<AttachProductToCategoryCommand>
    {
        private ILogger<AttachProductToCategoryCommandHandler> logger;
        private IProductRepository productRepository;
        public AttachProductToCategoryCommandHandler(ILogger<AttachProductToCategoryCommandHandler> logger, IProductRepository productRepository)
        {
            this.logger = logger;
            this.productRepository = productRepository;
        }

        public async Task<Result> Handle(AttachProductToCategoryCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                Result result = await this.productRepository.AddProductToCategory(command.ProductId, command.CategoryId);
                return Result.Ok();
            }
            catch(Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
