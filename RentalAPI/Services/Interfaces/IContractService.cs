﻿using RentalAPI.Models;
using RentalAPI.Services.OperationStatusEncapsulators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.Interfaces
{
    public interface IContractService:IBaseService<Contract>
    {
        public Task<DbOperationResponse<Contract>> UpdatePricesAsync(int contractId);
    }
}