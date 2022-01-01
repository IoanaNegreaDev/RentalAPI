using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class RentableRepository: GenericRepository<Rentable>, IRentableRepository

    {
        public RentableRepository(RentalDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Rentable>> ListAvailableAsync(int categoryId, 
                                                                    DateTime startDate, 
                                                                    DateTime endDate)
            => await _table.Where(rentable =>
                                            rentable.Category.Id == categoryId &&
                                            ((!rentable.Rentals
                                                .Select(rental => rental.RentedItemId)
                                                .Contains(rentable.Id)) ||
                                            rentable.Rentals.Where(rental =>
                                                rental.RentedItemId == rentable.Id &&
                                                (endDate < rental.StartDate ||
                                                    startDate > rental.EndDate))
                                                .Any()))
                                            .Include(item => item.Category)
                                            .ToListAsync();
    }
}
