using FluentValidation;
using RentalAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.ValidationFilters
{
    public class ContractCreationValidator: AbstractValidator<ContractCreationDTO>
    {
        public ContractCreationValidator()
        {
            RuleFor(item => item.Client.Name).NotEmpty().NotNull().MaximumLength(100);
            RuleFor(item => item.Client.Mobile).NotEmpty().NotNull().MaximumLength(100);
            RuleFor(item => item.PaymentCurrencyId).NotEmpty().NotNull().GreaterThan(0);
        }
    }
}
