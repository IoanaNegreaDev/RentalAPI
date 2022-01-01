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
    public class PaymentService:IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IPaymentRepository paymentRepository,
                                   IUnitOfWork unitOfWork)
        {
            this._paymentRepository = paymentRepository;
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Payment>> ListAsync()
        {
            return await _paymentRepository.ListAsync();
        }

        public async Task<PaymentOperationResponse> AddAsync(Payment damage)
        {
            try
            {
                await _paymentRepository.AddAsync(damage);
                await _unitOfWork.SaveChangesAsync();

                return new PaymentOperationResponse(damage);
            }
            catch (Exception ex)
            {
                return new PaymentOperationResponse("Failed to add payment to the database " + ex.Message);
            }
        }
        public async Task<Payment> FindByIdAsync(int id)
        {
            return await _paymentRepository.FindByIdAsync(id);
        }
    }
}
