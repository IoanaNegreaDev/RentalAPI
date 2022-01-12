using FluentValidation;
using RentalAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.ValidationFilters
{
    public class DamageCreationValidator: AbstractValidator<DamageCreationDTO>
    {
        public DamageCreationValidator()
        {
            RuleFor(item => item.OccuredInRentalId).NotEmpty().GreaterThan(0);
            RuleFor(item => item.DamageDescription).NotEmpty().MaximumLength(500);
            RuleFor(item => item.DamageCost).NotEmpty().GreaterThan(0).LessThan(Int32.MaxValue);
        }
    }
}
