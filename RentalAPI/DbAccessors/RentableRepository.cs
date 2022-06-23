using Microsoft.EntityFrameworkCore;
using RentalAPI.Controllers.ResourceParameters;
using RentalAPI.DbAccessors.SortingServices;
using RentalAPI.DTOs;
using RentalAPI.Extensions;
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
        IPropertyMappingService _propertyMappingService;
        public RentableRepository(RentalDbContext context, IPropertyMappingService propertyMappingService) : base(context)
        {
            _propertyMappingService = propertyMappingService;
        }

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
                    .ToListAsync();

        public async Task<PagedList<Rentable>> ListAsync(RentablesResourceParameters rentablesResourceParameters)
        {
            if (rentablesResourceParameters == null)
                return null;

            var collection = _table as IQueryable<Rentable>;

            if (!string.IsNullOrWhiteSpace(rentablesResourceParameters.category))
            {
                rentablesResourceParameters.category = rentablesResourceParameters.category.Trim();
                collection = collection.Where(rentable => rentable.Category.Name == rentablesResourceParameters.category)
                                       .Include(item => item.Category);
            }

            if (!string.IsNullOrWhiteSpace(rentablesResourceParameters.searchQuery))
            {
                rentablesResourceParameters.searchQuery = rentablesResourceParameters.searchQuery.Trim();
                collection = collection.Where(rentable =>
                                              rentable.Category.Name.Contains(rentablesResourceParameters.searchQuery) ||
                                              ((rentable as Vehicle) != null &&
                                                      ((rentable as Vehicle).Model.Contains(rentablesResourceParameters.searchQuery) ||
                                                       (rentable as Vehicle).Producer.Contains(rentablesResourceParameters.searchQuery) ||
                                                       (rentable as Vehicle).RegistrationNumber.Contains(rentablesResourceParameters.searchQuery) ||
                                                       (rentable as Vehicle).Fuel.Name.Contains(rentablesResourceParameters.searchQuery))) ||
                                              ((rentable as Sedan) != null &&
                                                       ((rentable as Sedan).Color.Contains(rentablesResourceParameters.searchQuery))));
            }

            if (!string.IsNullOrWhiteSpace(rentablesResourceParameters.OrderBy))
            {
                var rentablesPropertyMappingDictionary = _propertyMappingService.GetPropertyMapping<RentableDTO, Rentable>();
                //var vehicleRentablesPropertyMappingDictionary = _propertyMappingService.GetPropertyMapping<VehicleDTO, Vehicle>();

                collection = collection.ApplySort(rentablesResourceParameters.OrderBy, rentablesPropertyMappingDictionary);
               //  collection = collection.ApplySort(rentablesResourceParameters.OrderBy, vehicleRentablesPropertyMappingDictionary);
            }

            collection.Include(rentable => rentable.Category);

            return await PagedList<Rentable>.Create(collection, rentablesResourceParameters.PageNumber, rentablesResourceParameters.PageSize);
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
