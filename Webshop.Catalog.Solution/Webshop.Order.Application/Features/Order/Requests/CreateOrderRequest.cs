using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Order.Application.Features.Order.Requests
{
    public class CreateOrderRequest
    {
        public Customer Customer { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime DueDate { get; set; }
        public int Discount { get; set; }
        public Dictionary<Catalog.Domain.AggregateRoots.Product, int> OrderedProducts { get; set; }

        public class Validator : AbstractValidator<CreateOrderRequest>
        {
            public Validator()
            {
                /// <summary>
                /// Checks if the Customer object isn't null
                /// </summary>
                RuleFor(r => r.Customer)
                    .NotNull()
                    .WithMessage(Errors.General.ValueIsRequired(nameof(Customer)).Message);

                /// <summary>
                /// Checks if the Date of Issue has been set
                /// </summary>
                RuleFor(r => r.DateOfIssue)
                    .GreaterThan(DateTime.MinValue)
                    .WithMessage(Errors.General.ValueIsRequired(nameof(DateOfIssue)).Message);

                /*
                /// <summary>
                /// Checks if the Due Date has been set
                /// </summary>
                RuleFor(r => r.DueDate)
                    .GreaterThan(DateTime.MinValue)
                    .WithMessage(Errors.General.ValueIsRequired(nameof(DueDate)).Message);
                */

                /// <summary>
                /// Checks if the Discount is >= 0 && <= 15
                /// </summary>
                RuleFor(r => r.Discount)
                    .GreaterThan(-1)
                    .WithMessage(Errors.General.ValueTooSmall(nameof(Discount), 0).Message)
                    .LessThan(16)
                    .WithMessage(Errors.General.ValueTooLarge(nameof(Discount), 15).Message);

                /// <summary>
                /// Checks if the Ordered Items Dictionary contains at least one element (Product)
                /// </summary>
                RuleFor(r => r.OrderedProducts)
                    .Must(dictionary => dictionary != null && dictionary.Count > 0)
                    .WithMessage(Errors.General.ValueIsRequired(nameof(OrderedProducts)).Message);
            }
        }
    }
}
