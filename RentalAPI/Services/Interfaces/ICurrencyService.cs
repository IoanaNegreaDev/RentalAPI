﻿using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface ICurrencyService: IBasicService<Currency>
    {
        public Task<Currency> FindByNameAsync(string name);
        public Task<Currency> GetDefaultAsync();
    }
}
