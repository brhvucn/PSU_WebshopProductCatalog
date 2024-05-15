using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.Category.Application.Features.Category.Requests
{
    public class CreateCategoryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }

        public class Validator : AbstractValidator<CreateCategoryRequest>
        {
            public Validator()
            {
                RuleFor(r => r.Name).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(Name)).Code + " (" + Errors.General.ValueIsRequired(nameof(Name)).Message + ")");
                RuleFor(r => r.ParentId)
                    .NotNull().WithMessage(Errors.General.ValueIsRequired(nameof(ParentId)).Message)
                    .GreaterThan(0).WithMessage(Errors.General.ValueTooSmall(nameof(ParentId), 0).Message);
            }
        }
    }
}
