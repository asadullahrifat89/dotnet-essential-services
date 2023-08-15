namespace BaseModule.Application.DTOs.Responses
{
    public class ErrorResponse
    {
        public ErrorResponse()
        {

        }

        public string[] Errors { get; set; } = new string[] { };

        public ErrorResponse BuildExternalError(params string[] error)
        {
            return new ErrorResponse() { Errors = error is not null ? error.ToArray() : Array.Empty<string>() };
        }
    }
}
