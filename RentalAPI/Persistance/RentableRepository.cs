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

        public async Task<Rentable> FindByIdAsync(int categoryId, int id)
             => await _table.Where(item => item.Id == id && item.CategoryId == categoryId)
                          .Include(item => item.Category)
                          .Include(item => item.Damages)
                          .FirstOrDefaultAsync();

        override public async Task<Rentable> FindByIdAsync(int id)
            => await _table.Where(item => item.Id == id)
                          .Include(item => item.Category)
                          .Include(item => item.Damages)
                        //  .Include(item => item.Fuel)
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

        override public async Task<IEnumerable<Rentable>> ListAsync()
              => await _table
                    .Include(item => item.Category)
                    .Include(item => item.Damages)
                    .ToListAsync();

        public async Task<IEnumerable<Rentable>> ListAsync(int categoryId)
              => await _table
                    .Where(rentable => rentable.CategoryId == categoryId)   
                    .Include(item => item.Category)
                    .Include(item => item.Damages)
                    .ToListAsync();
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
