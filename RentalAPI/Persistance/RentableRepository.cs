using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class RentableRepository: BaseRepository, IRentableRepository

    {
        public RentableRepository(RentalDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Rentable>> ListAvailableAsync(int categoryId, 
                                                                    DateTime startDate, 
                                                                    DateTime endDate)
        {          
             return await _context.Rentables.Where(rentable =>
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

        public async Task<Rentable> FindByIdAsync(int id)
        {
            return await _context.Rentables.FindAsync(id);
        }

        /*   public async Task<IEnumerable<Rentable>> ListNotAvailableTrucksAsync(DateTime startDate, DateTime endDate)
           {
               return await _context.Rentables.Where(rentable =>
                                          rentable.Category.Name.ToLower().Contains("truck") &&
                                          rentable.Rentals.Where(rental =>
                                                                 rental.RentedItemId == rentable.Id &&
                                                                 (endDate >= rental.StartDate &&
                                                                  startDate <= rental.EndDate))
                                                               .Any())
                                          .ToListAsync();
           }*/

    }
}
