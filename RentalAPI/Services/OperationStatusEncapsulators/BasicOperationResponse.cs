using System;

namespace RentalAPI.Services.OperationStatusEncapsulators
{
    public class BasicOperationResponse<T> : BaseResponse where T : IComparable
    {
        public T _entity { get; private set; }

        private BasicOperationResponse(bool success, string message, T entity) : base(success, message)
        {
            _entity = entity;
        }

        public BasicOperationResponse(T entity) : this(true, string.Empty, entity)
        { }

        public BasicOperationResponse(string message) : base(false, message)
        { }
    }
}
