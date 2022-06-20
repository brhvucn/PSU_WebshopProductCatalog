using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Contracts.Persistence;
using Webshop.Catalog.Application.Features.Product.Dtos;
using Webshop.Domain.Common;

namespace Webshop.Catalog.Application.Features.Product.Queries.GetProduct
{
    public class GetProductQueryHandler : IQueryHandler<GetProductQuery, ProductDto>
    {
        private ILogger<GetProductQuery> logger;
        private IMapper mapper;
        private IProductRepository productRepository;
        public GetProductQueryHandler(ILogger<GetProductQuery> logger, IMapper mapper, IProductRepository productRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.productRepository = productRepository;
        }

        public Task<Result<ProductDto>> Handle(GetProductQuery query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
