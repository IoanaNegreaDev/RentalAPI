using RentalAPI.Models;
using RentalAPI.Persistance;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class DamageService:
        BaseService<Damage, IDamageRepository>, 
        IDamageService       
    {
        IRentalRepository _rentalRepository;
        public DamageService(IDamageRepository repository,
                             IRentalRepository rentalRepository,
                             IUnitOfWork unitOfWork)
            :base(repository, unitOfWork)
        {
            _rentalRepository = rentalRepository;
        }

        override public async Task<DbOperationResponse<Damage>> AddAsync(Damage item)
        {
            var dbRental = await _rentalRepository.FindByIdAsync(item.OccuredInRentalId);
            if (dbRental == null)
                return new DbOperationResponse<Damage>("RentalId not found in database.");

            item.RentableItemId = dbRental.RentedItemId;

            try
            {
                await _repository.AddAsync(item);
                await _unitOfWork.SaveChangesAsync();
                return new DbOperationResponse<Damage>(item);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<Damage>("Failed to add " + typeof(Damage).ToString() + " to the database " + ex.Message);
            }
        } 
    }
}
