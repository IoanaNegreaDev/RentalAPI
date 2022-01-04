using FluentValidation;
using RentalAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.ValidationFilters
{
    public class RentalCreationValidator: AbstractValidator<RentalCreationDTO>
    {

        public RentalCreationValidator()
        {
            RuleFor(item => item.RentedItemId).NotEmpty().Must(item => item > 0).WithMessage("RentedItemId must be greater than 0."); ;
            RuleFor(item => item.ContractId).NotEmpty().Must(item => item>0).WithMessage("ContractId must be greater than 0.");
            RuleFor(item => item.StartDate).NotEmpty().Must(item => item > DateTime.Today).WithMessage("StartDate must be greater than today."); 
            RuleFor(item => item.EndDate).NotEmpty().GreaterThan(item => item.StartDate).WithMessage("EndDate must be greater than StartDate."); ;
        }
    }
}
