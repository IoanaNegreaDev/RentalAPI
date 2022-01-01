using RentalAPI.Models;
using RentalAPI.Services.Interfaces;
using RentalAPI.Services.DbOperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RentalAPI.Persistance.Interfaces;

namespace RentalAPI.Services
{
    public class DamageService:IDamageService
    {
        private readonly IDamageRepository _damageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DamageService( IDamageRepository _damageRepository,
                                   IUnitOfWork unitOfWork)
        {
            this._damageRepository = _damageRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Damage>> ListAsync()
        {
            return await _damageRepository.ListAsync();
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
         public async Task<Damage> FindByIdAsync(int id)
        {
            return await _damageRepository.FindByIdAsync(id);
        }
    }
}
