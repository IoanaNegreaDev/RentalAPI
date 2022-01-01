using RentalAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.OperationStatuses
{
    public class ClientOperationResponse:BaseResponse
    {
        public Client _entity { get; private set; }

        private ClientOperationResponse(bool success, string message, Client entity) : base(success, message)
        {
            _entity = entity;
        }

        public ClientOperationResponse(Client entity) : this(true, string.Empty, entity)
        { }
    
        public ClientOperationResponse(string message) : this(false, message, null)
        { }
    }
}
