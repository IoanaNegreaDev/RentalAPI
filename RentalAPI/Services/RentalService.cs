using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class RentalService: BasicRentalService<Rental, IRentalRepository>, IRentalService
    {
        public RentalService(IRentalRepository rentalRepository,
                                IRentableRepository rentableRepository,
                                IContractRepository contractRepository,
                                IUnitOfWork unitOfWork) : base (rentalRepository, rentableRepository, contractRepository, unitOfWork)
        {
        }  
    }
}
