using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.Catalog.Application.Features.Product.Requests
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public string SKU { get; set; }
        public int Price { get; set; }

        /// <summary>
        /// The currency must be a 3 character currency code
        /// </summary>
        public string Currency { get; set; }

        public class Validator : AbstractValidator<CreateProductRequest>
        {
            public Validator()
            {
                //name
                RuleFor(r => r.Name)
                    .NotEmpty().WithMessage(Errors.General.ValueIsEmpty(nameof(Name)).Message);
                //sku
                RuleFor(r => r.SKU)
                    .NotEmpty().WithMessage(Errors.General.ValueIsEmpty(nameof(SKU)).Message);
                //price
                RuleFor(r => r.Price)
                    .NotEmpty().WithMessage(Errors.General.ValueIsEmpty(nameof(Price)).Message)
                    .GreaterThanOrEqualTo(0).WithMessage(Errors.General.ValueTooSmall(nameof(Price), 0).Message);
                //currency
                RuleFor(r => r.Currency)
                    .NotEmpty().WithMessage(Errors.General.ValueIsEmpty(nameof(Currency)).Message)
                    .Length(3).WithMessage(Errors.General.ValueOutOfRange(nameof(Currency), 3, 3).Message);
            }
        }
    }
}
