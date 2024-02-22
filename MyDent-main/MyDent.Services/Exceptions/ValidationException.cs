using System;

namespace MyDent.Services.Exceptions
{
    public class ValidationException : Exception
    {
        public int StatusCode { get; set; }
        public ValidationException(int statusCode)
        {
            StatusCode = statusCode;
        }

        public ValidationException(string message) : base(message)
        {

        }

        public ValidationException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public ValidationException(string message, Exception innerException, int statusCode) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
