using RentalAPI.Models;
using RentalAPI.Services.Interfaces;
using RentalAPI.Persistance.Interfaces;

namespace RentalAPI.Services
{
    public class DamageService:BaseService<Damage, IDamageRepository>, IDamageService
    {
        public DamageService(IDamageRepository repository, IUnitOfWork unitOfWork)
            :base(repository, unitOfWork)
        {
        }
    }
}
