using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using APIBase.Common.Constants;

namespace APIBase.Api.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ILogger _logger;

        protected BaseController(
            ILogger<BaseController> logger)
        {
            _logger = logger;
        }

        protected ObjectResult CreateOkResponse(
            object content = null)
        {
            var response = new ObjectResult(content ?? string.Empty)
            {
                StatusCode = (int)HttpStatusCode.OK
            };

            return response;
        }

        protected ObjectResult CreateInternalServerErrorResponse(
            object content = null)
        {
            var response = new ObjectResult(content ?? string.Empty)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            return response;
        }

        protected ObjectResult CreateCreatedResponse(
            object content = null)
        {
            var response = new ObjectResult(content ?? string.Empty)
            {
                StatusCode = (int)HttpStatusCode.Created
            };

            return response;
        }

        protected ObjectResult CreateAcceptedResponse(
            object content = null)
        {
            var response = new ObjectResult(content ?? string.Empty)
            {
                StatusCode = (int)HttpStatusCode.Accepted
            };

            return response;
        }

        protected ObjectResult CreateBadRequestResponse(
            object content = null)
        {
            var response = new ObjectResult(content ?? string.Empty)
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };

            return response;
        }

        protected ObjectResult CreateInvalidDataResponse(
            string message = null)
        {
            var response = new ObjectResult($"{BaseConstants.INVALID_DATA} {message}")
            {
                StatusCode = (int)HttpStatusCode.BadRequest
            };

            return response;
        }

        protected ObjectResult CreateUnauthorizedResponse(
            object content = null)
        {
            var response = new ObjectResult(content ?? string.Empty)
            {
                StatusCode = (int)HttpStatusCode.Unauthorized
            };

            return response;
        }

        protected ObjectResult CreateForbiddenResponse(
            object content = null)
        {
            var response = new ObjectResult(content ?? string.Empty)
            {
                StatusCode = (int)HttpStatusCode.Forbidden
            };

            return response;
        }

        protected ObjectResult CreateNotFoundResponse(
            object content = null)
        {
            var response = new ObjectResult(content ?? string.Empty)
            {
                StatusCode = (int)HttpStatusCode.NotFound
            };

            return response;
        }

        protected ObjectResult CreateMethodNotAllowedResponse(
            object content = null)
        {
            var response = new ObjectResult(content ?? string.Empty)
            {
                StatusCode = (int)HttpStatusCode.MethodNotAllowed
            };

            return response;
        }

        protected ObjectResult CreateRequestTimeoutResponse(
            object content = null)
        {
            var response = new ObjectResult(content ?? string.Empty)
            {
                StatusCode = (int)HttpStatusCode.RequestTimeout
            };

            return response;
        }

        protected ObjectResult CreateConflictResponse(
            object content = null)
        {
            var response = new ObjectResult(content ?? string.Empty)
            {
                StatusCode = (int)HttpStatusCode.Conflict
            };

            return response;
        }

        protected ObjectResult CreateBadGatewayResponse(
            object content = null)
        {
            var response = new ObjectResult(content ?? string.Empty)
            {
                StatusCode = (int)HttpStatusCode.BadGateway
            };

            return response;
        }

        protected ObjectResult CreateGatewayTimeoutResponse(
            object content = null)
        {
            var response = new ObjectResult(content ?? string.Empty)
            {
                StatusCode = (int)HttpStatusCode.GatewayTimeout
            };

            return response;
        }
    }
}
