using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.DbOperationStatusEncapsulators
{
    public class VehicleRentalOperationResponse:BaseResponse
    {
        public VehicleRental _entity { get; private set; }

        private VehicleRentalOperationResponse(bool success, string message, VehicleRental entity) : base(success, message)
        {
            _entity = entity;
        }

        public VehicleRentalOperationResponse(VehicleRental entity) : this(true, string.Empty, entity)
        { }

        public VehicleRentalOperationResponse(string message) : this(false, message, null)
        { }
    }
}
