using Microsoft.EntityFrameworkCore;
using RentalAPI.Models;
using RentalAPI.Persistance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance
{
    public class RentableRepository: GenericRepository<Vehicle>, IRentableRepository

    {
        public RentableRepository(RentalDbContext context) : base(context)
        {
        }

        override public async Task<IEnumerable<Vehicle>> ListAsync()
               => await _table
                            .Include(item =>item.Category)
                            .Include(item => item.Damages)
                            .Include(item =>item.Fuel)
                            .ToListAsync();
        override public async Task<Vehicle> FindByIdAsync(int id)
            => await _table.Where(item => item.Id == id)
                          .Include(item => item.Category)
                          .Include(item => item.Damages)
                          .Include(item => item.Fuel)
                          .FirstOrDefaultAsync();                           

        public async Task<bool> IsAvailable(int id, DateTime startDate, DateTime endDate)
               => await _table.Where(rentable =>
                                            rentable.Id == id &&
                                            ((!rentable.Rentals
                                                .Select(rental => rental.RentedItemId)
                                                .Contains(rentable.Id)) ||
                                            rentable.Rentals.Where(rental =>
                                                rental.RentedItemId == rentable.Id &&
                                                (endDate < rental.StartDate ||
                                                    startDate > rental.EndDate))
                                                .Any()))
                                            .AnyAsync();

        public async Task<IEnumerable<Vehicle>> ListAvailableAsync(int categoryId, 
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
