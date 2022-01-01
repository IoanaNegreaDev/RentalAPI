using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.DbOperationStatusEncapsulators
{
    public class PaymentOperationResponse: BaseResponse
    {
        public Payment _entity { get; private set; }

        private PaymentOperationResponse(bool success, string message, Payment entity) : base(success, message)
        {
            _entity = entity;
        }

        public PaymentOperationResponse(Payment entity) : this(true, string.Empty, entity)
        { }

        public PaymentOperationResponse(string message) : this(false, message, null)
        { }
    }
}
