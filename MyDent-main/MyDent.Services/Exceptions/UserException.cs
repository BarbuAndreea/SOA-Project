using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyDent.Services.Exceptions
{
    public class UserException : Exception
    {
        public int StatusCode { get; set; }
        public UserException(int statusCode)
        {
            StatusCode = statusCode;
        }

        public UserException(string message, int statusCode) : base(message)
        {
            StatusCode = statusCode;
        }

        public UserException(string message, Exception innerException, int statusCode) : base(message, innerException)
        {
            StatusCode = statusCode;
        }
    }
}
