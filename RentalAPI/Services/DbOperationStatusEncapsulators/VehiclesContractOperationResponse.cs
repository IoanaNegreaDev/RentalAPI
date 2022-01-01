using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.DbOperationStatusEncapsulators
{
    public class VehicleContractOperationResponse : BaseResponse
    {
        public VehicleContract _entity { get; private set; }

        private VehicleContractOperationResponse(bool success, string message, VehicleContract entity) : base(success, message)
        {
            _entity = entity;
        }

        public VehicleContractOperationResponse(VehicleContract entity) : this(true, string.Empty, entity)
        { }

        public VehicleContractOperationResponse(string message) : this(false, message, null)
        { }
    }
}
