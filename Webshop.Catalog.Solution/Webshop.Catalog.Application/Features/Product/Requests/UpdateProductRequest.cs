using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.Catalog.Application.Features.Product.Requests
{
    public class UpdateProductRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SKU { get; set; }
        public int AmountInStock { get; set; }
        public int Price { get; set; }
        public string Currency { get; set; }
        public int MinStock { get; set; }

        public class Validator : AbstractValidator<UpdateProductRequest>
        {
            public Validator()
            {
                //Id
                RuleFor(r=>r.Id)
                    .NotEmpty().WithMessage(Errors.General.ValueIsEmpty(nameof(Id)).Message)
                    .GreaterThanOrEqualTo(0).WithMessage(Errors.General.ValueTooSmall(nameof(Id), 0).Message);
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
                //amount in stock
                RuleFor(r => r.AmountInStock)
                    .NotEmpty().WithMessage(Errors.General.ValueIsEmpty(nameof(AmountInStock)).Message)
                    .GreaterThanOrEqualTo(0).WithMessage(Errors.General.ValueTooSmall(nameof(AmountInStock), 0).Message);
                //min amount in stock
                RuleFor(r => r.MinStock)
                    .NotEmpty().WithMessage(Errors.General.ValueIsEmpty(nameof(MinStock)).Message)
                    .GreaterThanOrEqualTo(0).WithMessage(Errors.General.ValueTooSmall(nameof(MinStock), 0).Message);
            }
        }
    }
}
