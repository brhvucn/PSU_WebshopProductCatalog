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

namespace Webshop.Catalog.Application.Features.Product.Queries.GetProducts
{
    public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, IEnumerable<ProductDto>>
    {
        private ILogger<GetProductsQueryHandler> logger;
        private IMapper mapper;
        private IProductRepository productRepository;
        public GetProductsQueryHandler(ILogger<GetProductsQueryHandler> logger, IMapper mapper, IProductRepository productRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.productRepository = productRepository;
        }

        public Task<Result<IEnumerable<ProductDto>>> Handle(GetProductsQuery query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
