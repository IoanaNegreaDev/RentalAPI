using RentalAPI.Models;

namespace RentalAPI.Services.Interfaces
{
    public interface IPriceResolverService
    {
        public float ExtractTotalBasePrice(Contract contract);
        public float ExtractTotalDamagePrice(Contract contract);
        public float ExtractTotalExtraCharges(Contract contract);
    }
}
