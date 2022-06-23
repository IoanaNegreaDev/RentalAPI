using FluentValidation;
using RentalAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.ValidationFilters
{
    public class UserRegistrationValidator : AbstractValidator<UserRegistrationDTO>
    {
        public UserRegistrationValidator()
        {
            RuleFor(item => item.UserName).NotEmpty().NotNull().MaximumLength(100);
            RuleFor(item => item.Password).NotEmpty().NotNull().MaximumLength(100);
        }
    }
}
