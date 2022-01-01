using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IPaymentRepository
    {
        public Task<IEnumerable<Payment>> ListAsync();
        public Task<Payment> FindByIdAsync(int id);
        public Task AddAsync(Payment contract);
    }
}
