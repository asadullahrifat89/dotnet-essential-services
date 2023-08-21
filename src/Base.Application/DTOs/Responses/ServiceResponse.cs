using System.Net;

namespace Base.Application.DTOs.Responses
{
    public class ServiceResponse
    {
        public string RequestUri { get; set; } = string.Empty;

        public string ExternalError { get; set; } = string.Empty;

        public HttpStatusCode HttpStatusCode { get; set; } = HttpStatusCode.OK;

        public object Result { get; set; } = new object();

        public bool IsSuccess => HttpStatusCode == HttpStatusCode.OK;

        public ServiceResponse BuildSuccessResponse(object result, string requestUri = "")
        {
            return new ServiceResponse
            {
                HttpStatusCode = HttpStatusCode.OK,
                Result = result,
                RequestUri = requestUri,
            };
        }

        public ServiceResponse BuildErrorResponse(string error, string requestUri = "")
        {
            return new ServiceResponse
            {
                HttpStatusCode = HttpStatusCode.InternalServerError,
                ExternalError = error,
                RequestUri = requestUri,
            };
        }

        public ServiceResponse BuildBadRequestResponse(string error, string requestUri = "")
        {
            return new ServiceResponse()
            {
                HttpStatusCode = HttpStatusCode.BadRequest,
                ExternalError = error,
                RequestUri = requestUri
            };
        }
    }
}
