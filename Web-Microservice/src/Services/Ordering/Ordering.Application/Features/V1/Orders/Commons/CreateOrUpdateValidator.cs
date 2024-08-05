using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Features.V1.Orders
{
    public class CreateOrUpdateValidator : AbstractValidator<CreateOrUpdateCommand>
    {
        public CreateOrUpdateValidator()
        {
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("{FirstName} is reqired")
                .NotNull()
                .MaximumLength(50).WithMessage("{FirstName} max is 50 character");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("{LastName} is reqired")
                .NotNull()
                .MaximumLength(50).WithMessage("{LastName} max is 50 character");

            RuleFor(p => p.EmailAddress)
                .EmailAddress().WithMessage("{EmailAddress} is valid format")
                .NotEmpty().WithMessage("{EmailAddress} is required");

            RuleFor(p => p.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} is required")
                .GreaterThan(0).WithMessage("{TotalPrice} must be greater 0");
        }
    }
}
