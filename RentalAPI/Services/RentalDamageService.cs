using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.DbOperationStatusEncapsulators;
using RentalAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services
{
    public class RentalDamageService:IRentalDamageService
    {
        private readonly IRentalDamageRepository _rentalDamageRepository;
        private readonly IDamageRepository _damageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RentalDamageService(IRentalDamageRepository _rentalDamageRepository,
                                   IDamageRepository _damageRepository,
                                   IUnitOfWork unitOfWork)
        {
            this._rentalDamageRepository = _rentalDamageRepository;
            this._damageRepository = _damageRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<RentalDamage>> ListAsync()
        {
            return await _rentalDamageRepository.ListAsync();
        }

        public async Task<DamageOperationResponse> AddAsync(Damage damage)
        {
            try
            {
                await _damageRepository.AddAsync(damage);
                await _unitOfWork.SaveChangesAsync();

                return new DamageOperationResponse(damage);
            }
            catch (Exception ex)
            {
                return new DamageOperationResponse("Failed to add damage to the database " + ex.Message);
            }
        }
        public async Task<RentalDamageOperationResponse> AddAsync(RentalDamage rentalDamage)
        {
            try
            {
                await _rentalDamageRepository.AddAsync(rentalDamage);
                await _unitOfWork.SaveChangesAsync();

                return new RentalDamageOperationResponse(rentalDamage);
            }
            catch (Exception ex)
            {
                return new RentalDamageOperationResponse("Failed to add rental damage to the database " + ex.Message);
            }
        }

        public async Task<RentalDamage> FindByIdAsync(int id)
        {
            return await _rentalDamageRepository.FindByIdAsync(id);
        }
    }
}
