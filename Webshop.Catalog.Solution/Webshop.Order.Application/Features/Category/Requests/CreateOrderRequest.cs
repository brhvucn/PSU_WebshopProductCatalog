using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.AggregateRoots;
using Webshop.Domain.Common;

namespace Webshop.Order.Application.Features.Category.Requests
{
    public class CreateOrderRequest
    {
        public Customer Customer { get; set; }
        public DateTime DateOfIssue { get; set; }
        public DateTime DueDate { get; set; }
        public Dictionary<Catalog.Domain.AggregateRoots.Product, int> OrderedProducts { get; set; }

        public class Validator : AbstractValidator<CreateOrderRequest>
        {
            public Validator()
            { 
                //Checking if the Customer object isn't null
                RuleFor(r => r.Customer)
                    .NotNull()
                    .WithMessage(Errors.General.ValueIsRequired(nameof(Customer)).Message);
                //Checking if the Date of Issue has been set
                RuleFor(r => r.DateOfIssue)
                    .GreaterThan(DateTime.MinValue)
                    .WithMessage(Errors.General.ValueIsRequired(nameof(DateOfIssue)).Message);
                /*
                //Checking if the Due Date has been set
                RuleFor(r => r.DueDate)
                    .GreaterThan(DateTime.MinValue)
                    .WithMessage(Errors.General.ValueIsRequired(nameof(DueDate)).Message);
                */
                //Checking if the Ordered Items Dictionary contains at least one element (Product)
                RuleFor(r => r.OrderedProducts)
                    .Must(dictionary => dictionary != null && dictionary.Count > 0)
                    .WithMessage(Errors.General.ValueIsRequired(nameof(OrderedProducts)).Message);
            }
        }
    }
}
