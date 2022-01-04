using FluentValidation;
using RentalAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.ValidationFilters
{
    public class UpdateClientValidator : AbstractValidator<ClientUpdateDTO>
    {
        public UpdateClientValidator()
        {
            RuleFor(item => item.Name).MaximumLength(100);
            RuleFor(item => item.Mobile).MaximumLength(100);
        }
    }
}
