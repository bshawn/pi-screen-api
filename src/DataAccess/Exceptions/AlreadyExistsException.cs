using System;

namespace ScreenApi.DataAccess.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException() : base() { }
        public AlreadyExistsException(string message) : base(message) { }
    }
}