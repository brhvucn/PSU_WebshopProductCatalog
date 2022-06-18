using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.Category.Application.Features.Category.Requests
{
    public class UpdateCategoryRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public class Validator : AbstractValidator<UpdateCategoryRequest>
        {
            public Validator()
            {
                RuleFor(r => r.Name).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(Name)).Code + " (" + Errors.General.ValueIsRequired(nameof(Name)).Message + ")");
            }
        }
    }
}
