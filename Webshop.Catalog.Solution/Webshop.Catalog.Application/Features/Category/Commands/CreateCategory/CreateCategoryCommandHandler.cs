using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Webshop.Application.Contracts;
using Webshop.Catalog.Application.Contracts.Persistence;
using Webshop.Catalog.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Category.Application.Features.Category.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand>
    {
        private ILogger<CreateCategoryCommandHandler> logger;
        private ICategoryRepository categoryRepository;
        public CreateCategoryCommandHandler(ILogger<CreateCategoryCommandHandler> logger, ICategoryRepository categoryRepository)
        {
            this.logger = logger;
            this.categoryRepository = categoryRepository;
        }

        public async Task<Result> Handle(CreateCategoryCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                if (command.ParentId > 0) //0 is a root category, and is already allowed
                {
                    bool existsParentCategory = await categoryRepository.ExistsCategory(command.ParentId);
                    if (!existsParentCategory)
                    {
                        throw new ArgumentOutOfRangeException(nameof(command.ParentId), "A category with the provided parentId does not exist");
                    }
                }
                Catalog.Domain.AggregateRoots.Category category = new Catalog.Domain.AggregateRoots.Category(command.Name);
                category.Description = command.Description;
                category.ParentId = command.ParentId;
                await this.categoryRepository.CreateAsync(category);
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
