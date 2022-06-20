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

namespace Webshop.Catalog.Application.Features.Product.Commands.CreateProduct
{
    public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand>
    {
        private ILogger<CreateProductCommand> logger;
        private IProductRepository repository;
        public CreateProductCommandHandler(ILogger<CreateProductCommand> logger, IProductRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        public Task<Result> Handle(CreateProductCommand command, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
