using RentalAPI.Models;
using RentalAPI.Persistance;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class RentalDamageService:
        BaseService<RentalDamage, IRentalDamageRepository>, 
        IRentalDamageService       
    {
        public RentalDamageService(IRentalDamageRepository repository,
                                   IUnitOfWork unitOfWork)
            :base(repository, unitOfWork)
        {
        }
    }
}
