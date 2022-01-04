using FluentValidation;
using RentalAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.ValidationFilters
{
    public class RentalDamageCreationValidator: AbstractValidator<RentalDamageCreationDTO>
    {
        public RentalDamageCreationValidator()
        {
            RuleFor(item => item.RentalId).NotEmpty().GreaterThan(0);
            RuleFor(item => item.Damage.DamageDescription).NotEmpty().MaximumLength(500);
            RuleFor(item => item.Damage.DamageCost).NotEmpty().GreaterThan(0).LessThan(Int32.MaxValue);
        }
    }
}
