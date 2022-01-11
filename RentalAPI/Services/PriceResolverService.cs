using RentalAPI.Models;
using RentalAPI.Services.Interfaces;
using System.Linq;

namespace RentalAPI.Services
{
    public class PriceResolverService: IPriceResolverService
    {
        public PriceResolverService()
        { }

        public float ExtractTotalDamagePrice(Contract contract)
           => contract.Rentals
                    .Where(item => item != null)
                    .Sum(item => (float)(item.RentalDamages
                                             .Where(item => item.Damage != null)
                                             .Sum(item => item.Damage.DamageCost)))
                    * contract.ExchangeRate;

        public float ExtractTotalBasePrice(Contract contract)
             => contract.Rentals
                    .Where(item => item != null)
                    .Sum(item => item.BasePrice)
                    * contract.ExchangeRate;
        public float ExtractTotalExtraCharges(Contract contract)
        {
            return contract.Rentals
                      .Where(rental => rental != null &&
                                       rental.GetType() == typeof(VehicleRental) &&
                                        ((VehicleRental)rental).FullTank == false)
                      .Sum(rental => ((VehicleRental)rental).FullTankPrice)
                      * contract.ExchangeRate;
        }
     }
}
