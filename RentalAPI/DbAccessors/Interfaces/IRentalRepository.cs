﻿using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Persistance.Interfaces
{
    public interface IRentalRepository: IGenericRepository<Rental>
    {
        Task<Rental> FindByIdAsync(int contractId, int id);
        Task<IEnumerable<Rental>> ListAsync(int contractId);

        Task<Rental> FindByIdAsync(string userId, int contractId, int id);
        Task<IEnumerable<Rental>> ListAsync(string userId, int contractId);
    }
}
