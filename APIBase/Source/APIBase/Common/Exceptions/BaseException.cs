using System;
using System.Net;

namespace APIBase.Common.Exceptions
{
    public abstract class BaseException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        
        protected BaseException(
            string message)
            :base(message)
        {
        }

        protected BaseException(
            string message,
            HttpStatusCode statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
