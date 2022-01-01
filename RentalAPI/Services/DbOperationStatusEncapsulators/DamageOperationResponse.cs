using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.DbOperationStatusEncapsulators
{
    public class DamageOperationResponse: BaseResponse
    {
        public Damage _entity { get; private set; }

        private DamageOperationResponse(bool success, string message, Damage entity) : base(success, message)
        {
            _entity = entity;
        }

        public DamageOperationResponse(Damage entity) : this(true, string.Empty, entity)
        { }

        public DamageOperationResponse(string message) : this(false, message, null)
        { }
    }
}
