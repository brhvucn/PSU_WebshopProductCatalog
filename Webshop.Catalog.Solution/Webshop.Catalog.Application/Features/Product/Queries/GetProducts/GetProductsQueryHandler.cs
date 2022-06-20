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

        public async Task<Result<IEnumerable<ProductDto>>> Handle(GetProductsQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                IEnumerable<Domain.AggregateRoots.Product> products = await this.productRepository.GetAllFromCategory(query.CategoryId);
                return Result.Ok(this.mapper.Map<IEnumerable<ProductDto>>(products));
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail<IEnumerable<ProductDto>>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
