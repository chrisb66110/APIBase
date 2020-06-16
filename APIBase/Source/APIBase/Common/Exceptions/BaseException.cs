using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace APIBase.Common.Exceptions
{
    [ExcludeFromCodeCoverage]
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
