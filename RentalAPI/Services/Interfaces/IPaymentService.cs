using RentalAPI.Models;
using RentalAPI.Services.DbOperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IPaymentService
    {
        public Task<IEnumerable<Payment>> ListAsync();
        public Task<PaymentOperationResponse> AddAsync(Payment payment);
        public Task<Payment> FindByIdAsync(int id);

    }
}
