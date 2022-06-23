using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;
using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private async Task<BasicOperationResponse<bool>> CheckConsistencyAsync(int contractId, int rentalId)
        {
            var contract = await _contractRepository.FindByIdAsync(contractId);
            if (contract == null)
                return new BasicOperationResponse<bool>("Contract not found.");

            var contractContainsRental = contract.Rentals.Any(rental => rental.Id == rentalId);
            if (!contractContainsRental)
                return new BasicOperationResponse<bool>("Invalid rentalId for this contract.");

            return new BasicOperationResponse<bool>(true);
        }

        private async Task<BasicOperationResponse<bool>> CheckConsistencyAsync(string userId, int contractId, int rentalId)
        {
            var contract = await _contractRepository.FindByIdAsync(contractId);
            if (contract == null)
                return new BasicOperationResponse<bool>("Contract not found.");

            if (userId != contract.User.Id)
                return new BasicOperationResponse<bool>("Invalid contractId for this user.");

            var contractContainsRental = contract.Rentals.Any(rental => rental.Id == rentalId);
            if (!contractContainsRental)
                return new BasicOperationResponse<bool>("Invalid rentalId for this contract.");

            return new BasicOperationResponse<bool>(true);
        }

        public async Task<DbOperationResponse<Damage>> FindByIdAsync(int contractId, int rentalId, int damageId)
        {
            var consistencyCheckResult = await CheckConsistencyAsync(contractId, rentalId);
            if (!consistencyCheckResult.Success)
                return new DbOperationResponse<Damage>(consistencyCheckResult.Message);

            var damage = await _repository.FindByIdAsync(contractId, rentalId, damageId);

            return new DbOperationResponse<Damage>(damage);
        }

        public async Task<DbOperationResponse<Damage>> FindByIdAsync(string userId, int contractId, int rentalId, int damageId)
        {
            var consistencyCheckResult = await CheckConsistencyAsync(userId, contractId, rentalId);
            if (!consistencyCheckResult.Success)
                return new DbOperationResponse<Damage>(consistencyCheckResult.Message);

            var damage = await _repository.FindByIdAsync(contractId, rentalId, damageId);

            return new DbOperationResponse<Damage>(damage);
        }

        public async Task<DbOperationResponse<IEnumerable<Damage>>> ListAsync(int contractId, int rentalId)
        {
            var consistencyCheckResult = await CheckConsistencyAsync(contractId, rentalId);
            if (!consistencyCheckResult.Success)
                return new DbOperationResponse<IEnumerable<Damage>>(consistencyCheckResult.Message);

            var damages = await _repository.ListAsync(contractId, rentalId);
            if (damages == null)
                return new DbOperationResponse<IEnumerable<Damage>>("No damages.");

            return new DbOperationResponse<IEnumerable<Damage>>(damages);
        }

        public async Task<DbOperationResponse<IEnumerable<Damage>>> ListAsync(string userId, int contractId, int rentalId)
        {
            var consistencyCheckResult = await CheckConsistencyAsync(userId, contractId, rentalId);
            if (!consistencyCheckResult.Success)
                return new DbOperationResponse<IEnumerable<Damage>>(consistencyCheckResult.Message);

            var damages = await _repository.ListAsync(contractId, rentalId);

            return new DbOperationResponse<IEnumerable<Damage>>(damages);
        }

        override public async Task<DbOperationResponse<Damage>> AddAsync(Damage damage)
        {
            if (damage == null)
                return new DbOperationResponse<Damage>("null Damage object.");

            var dbRental = await _rentalRepository.FindByIdAsync(damage.OccuredInRentalId);
            if (dbRental == null)
                return new DbOperationResponse<Damage>("Rental not found in database.");

            damage.RentableItemId = dbRental.RentedItemId;
            damage.OccuredInRentalId = dbRental.Id;

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

        public async Task<DbOperationResponse<Damage>> UpdateAsync(int contractId, int rentalId, Damage damage)
        {
            var consistencyCheckResult = await CheckConsistencyAsync(contractId, rentalId);
            if (!consistencyCheckResult.Success)
                return new DbOperationResponse<Damage>(consistencyCheckResult.Message);

            damage.OccuredInRentalId = rentalId;
            
            _repository.Update(damage);

            return new DbOperationResponse<Damage>(damage);
        }
  
        public async Task<DbOperationResponse<Damage>> DeleteAsync(int contractId, int rentalId, int damageId)
        {
            var consistencyCheckResult = await CheckConsistencyAsync(contractId, rentalId);
            if (!consistencyCheckResult.Success)
                return new DbOperationResponse<Damage>(consistencyCheckResult.Message);

            var damage = await _repository.FindByIdAsync(contractId, rentalId, damageId);
            if (damage == null)
                return new DbOperationResponse<Damage>("Damage not found.");

            _repository.Remove(damage);

            return new DbOperationResponse<Damage>("Delete was successful.");
        }     
    }
}
