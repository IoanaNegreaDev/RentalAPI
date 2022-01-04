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
        IVehicleRentalRepository _rentalRepository;
        public RentalDamageService(IRentalDamageRepository repository,
                                   IVehicleRentalRepository rentalRepository,
                                   IUnitOfWork unitOfWork)
            :base(repository, unitOfWork)
        {
            _rentalRepository = rentalRepository;
        }

        override public async Task<DbOperationResponse<RentalDamage>> AddAsync(RentalDamage item)
        {
            var dbRental = await _rentalRepository.FindByIdAsync(item.RentalId);
            if (dbRental == null)
                return new DbOperationResponse<RentalDamage>("RentalId not found in database.");

            item.Damage.RentableItemId = dbRental.RentedItemId;

            try
            {
                await _repository.AddAsync(item);
                await _unitOfWork.SaveChangesAsync();
                return new DbOperationResponse<RentalDamage>(item);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<RentalDamage>("Failed to add " + typeof(RentalDamage).ToString() + " to the database " + ex.Message);
            }
        } 
    }
}
