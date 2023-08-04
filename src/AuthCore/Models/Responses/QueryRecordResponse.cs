namespace BaseCore.Models.Responses
{
    public class QueryRecordResponse<TRecord>
    {
        public string RequestUri { get; set; } = string.Empty;

        public ErrorResponse Errors { get; set; } = new ErrorResponse();

        public TRecord? Result { get; set; }

        public bool IsSuccess => Errors.Errors is null || Errors.Errors.Count() == 0;

        public QueryRecordResponse<TRecord> BuildSuccessResponse(TRecord result, string requestUri = "")
        {
            return new QueryRecordResponse<TRecord>()
            {
                Result = result,
                RequestUri = requestUri
            };
        }

        public QueryRecordResponse<TRecord> BuildErrorResponse(ErrorResponse errors, string requestUri = "")
        {
            return new QueryRecordResponse<TRecord>()
            {
                Errors = errors,
                RequestUri = requestUri
            };
        }
    }
}
