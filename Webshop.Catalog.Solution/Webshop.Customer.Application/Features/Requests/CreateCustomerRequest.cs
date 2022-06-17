using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.Domain.Common;

namespace Webshop.Customer.Application.Features.Requests
{
    public class CreateCustomerRequest
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }

        public class Validator : AbstractValidator<CreateCustomerRequest>
        {
            public Validator()
            {
                //business rules for creating
                //cannot be empty
                RuleFor(r=>r.Name).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(Name)).Code);
                RuleFor(r=>r.Address).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(Name)).Code);                
                RuleFor(r => r.City).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(Name)).Code);
                RuleFor(r => r.Region).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(Name)).Code);
                RuleFor(r => r.PostalCode).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(Name)).Code);
                RuleFor(r => r.Country).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(Name)).Code);
                RuleFor(r => r.Email).NotEmpty().WithMessage(Errors.General.ValueIsRequired(nameof(Name)).Code);
                //email must be an email
                RuleFor(r => r.Email).EmailAddress().WithMessage(Errors.General.UnexpectedValue(nameof(Email)).Code);
            }
        }
    }
}
