using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class DamageService:
        BasicService<Damage, IDamageRepository>, 
        IDamageService       
    {
        IRentalRepository _rentalRepository;
        IContractRepository _contractRepository;
        public DamageService(IDamageRepository repository,
                             IRentalRepository rentalRepository,
                             IContractRepository contractRepository,
                             IUnitOfWork unitOfWork)
            :base(repository, unitOfWork)
        {
            _rentalRepository = rentalRepository;
            _contractRepository = contractRepository;
        }


        override public async Task<DbOperationResponse<Damage>> AddAsync(Damage damage)
        {
            var dbRental = await _rentalRepository.FindByIdAsync(damage.OccuredInRentalId);
            if (dbRental == null)
                return new DbOperationResponse<Damage>("Rental not found in database.");

            damage.RentableItemId = dbRental.RentedItemId;

            try
            {
                await _repository.AddAsync(damage);
                await _unitOfWork.SaveChangesAsync();
                return new DbOperationResponse<Damage>(damage);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<Damage>("Failed to add " + typeof(Damage).ToString() + " to the database " + ex.Message);
            }
        }

        public async Task<DbOperationResponse<Damage>> AddAsync(int contractId, int rentalId, Damage damage)
        {
            var dbRental = await _rentalRepository.FindByIdAsync(contractId, rentalId);
            if (dbRental == null)
                return new DbOperationResponse<Damage>("Rental not found in database.");

            damage.OccuredInRentalId = rentalId;
            damage.RentableItemId = dbRental.RentedItemId;

            try
            {
                await _repository.AddAsync(damage);
                await _unitOfWork.SaveChangesAsync();
                return new DbOperationResponse<Damage>(damage);
            }
            catch (Exception ex)
            {
                return new DbOperationResponse<Damage>("Failed to add " + typeof(Damage).ToString() + " to the database " + ex.Message);
            }
        }

        public async Task<DbOperationResponse<IEnumerable<Damage>>> ListAsync(int contractId, int rentalId)
        {
            var contract = await _contractRepository.FindByIdAsync(contractId);
            if (contract == null)
                return new DbOperationResponse<IEnumerable<Damage>>("Contract not found.");

            var rental = await _rentalRepository.FindByIdAsync(contractId, rentalId);
            if (rental == null)
                return new DbOperationResponse<IEnumerable<Damage>>("Rental not found.");

            var damages = await _repository.ListAsync(contractId, rentalId);
            if (damages == null)
                return new DbOperationResponse<IEnumerable<Damage>>("No damages.");

            return new DbOperationResponse<IEnumerable<Damage>>(damages);
        }

        public async Task<DbOperationResponse<Damage>> FindByIdAsync(int contractId, int rentalId, int damageId)
        {
            var contract = await _contractRepository.FindByIdAsync(contractId);
            if (contract == null)
                return new DbOperationResponse<Damage>("Contract not found.");

            var rental = await _rentalRepository.FindByIdAsync(contractId, rentalId);
            if (rental == null)
                return new DbOperationResponse<Damage> ("Rental not found.");
            var damage = await _repository.FindByIdAsync(contractId, rentalId, damageId);

            return new DbOperationResponse<Damage>(damage);
        }
        public async Task<DbOperationResponse<Damage>> UpdateAsync(int contractId, int rentalId, Damage damage)
        {
            var contract = await _contractRepository.FindByIdAsync(contractId);
            if (contract == null)
                return new DbOperationResponse<Damage>("Contract not found.");

            var rental = await _rentalRepository.FindByIdAsync(contractId, rentalId);
            if (rental == null)
                return new DbOperationResponse<Damage>("Rental not found.");

            damage.OccuredInRentalId = rentalId;
            
            _repository.Update(damage);

            return new DbOperationResponse<Damage>(damage);
        }
        public async Task<DbOperationResponse<Damage>> DeleteAsync(int contractId, int rentalId, int damageId)
        {
            var contract = await _contractRepository.FindByIdAsync(contractId);
            if (contract == null)
                return new DbOperationResponse<Damage>("Contract not found.");

            var rental = await _rentalRepository.FindByIdAsync(contractId, rentalId);
            if (rental == null)
                return new DbOperationResponse<Damage>("Rental not found.");

            var damage = await _repository.FindByIdAsync(contractId, rentalId, damageId);
            if (damage == null)
                return new DbOperationResponse<Damage>("Damage not found.");

            _repository.Remove(damage);

            return new DbOperationResponse<Damage>("Delete was successful.");
        }
    }
}
