using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.DbOperationStatusEncapsulators
{
    public class RentalDamageOperationResponse: BaseResponse
    {
        public RentalDamage _entity { get; private set; }

        private RentalDamageOperationResponse(bool success, string message, RentalDamage entity) : base(success, message)
        {
            _entity = entity;
        }

        public RentalDamageOperationResponse(RentalDamage entity) : this(true, string.Empty, entity)
        { }

        public RentalDamageOperationResponse(string message) : this(false, message, null)
        { }
    }
}
