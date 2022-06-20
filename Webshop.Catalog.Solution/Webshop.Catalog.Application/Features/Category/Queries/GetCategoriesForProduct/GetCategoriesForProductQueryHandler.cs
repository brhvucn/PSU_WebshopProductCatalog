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
using Webshop.Category.Application.Features.Category.Dtos;
using Webshop.Domain.Common;

namespace Webshop.Catalog.Application.Features.Category.Queries.GetCategoriesForProduct
{
    public class GetCategoriesForProductQueryHandler : IQueryHandler<GetCategoriesForProductQuery, IEnumerable<CategoryDto>>
    {
        private ILogger<GetCategoriesForProductQueryHandler> logger;
        private IMapper mapper;
        private ICategoryRepository categoryRepository;
        public GetCategoriesForProductQueryHandler(ILogger<GetCategoriesForProductQueryHandler> logger, IMapper mapper, ICategoryRepository categoryRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.categoryRepository = categoryRepository;
        }

        public async Task<Result<IEnumerable<CategoryDto>>> Handle(GetCategoriesForProductQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await this.categoryRepository.GetForProduct(query.ProductId);
                return Result.Ok(this.mapper.Map<IEnumerable<CategoryDto>>(result));
            }
            catch (Exception ex)
            {
                this.logger.LogCritical(ex, ex.Message);
                return Result.Fail<IEnumerable<CategoryDto>>(Errors.General.UnspecifiedError(ex.Message));
            }
        }
    }
}
