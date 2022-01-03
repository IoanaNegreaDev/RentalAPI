using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using RentalAPI.Services.Interfaces;

namespace RentalAPI.Services
{
    public class PaymentService:BaseService<Payment, IGenericRepository<Payment>>, IPaymentService
    {
        public PaymentService(IGenericRepository<Payment> repository, IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }
    }
}
