using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentalAPI.Services.OperationStatusEncapsulators
{
    public class DbOperationResponse <T>: BaseResponse where T :class
    {
        public T _entity { get; private set; }

        private DbOperationResponse(bool success, string message, T entity) : base(success, message)
        {
            _entity = entity;
        }

        public DbOperationResponse(T entity) : this(true, string.Empty, entity)
        { }

        public DbOperationResponse(string message) : this(false, message, null)
        { }
    }
}
