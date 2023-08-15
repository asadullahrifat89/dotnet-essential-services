namespace BaseModule.Domain.DTOs.Responses
{
    public class QueryRecordsResponse<TRecord>
    {
        public string RequestUri { get; set; } = string.Empty;

        public ErrorResponse Errors { get; set; } = new ErrorResponse();

        public QueryResult<TRecord> Result { get; set; } = new QueryResult<TRecord>();

        public bool IsSuccess => Errors.Errors is null || Errors.Errors.Count() == 0;

        public QueryRecordsResponse<TRecord> BuildSuccessResponse(
            long count,
            TRecord[] records,
            string requestUri = "")
        {
            return new QueryRecordsResponse<TRecord>()
            {
                RequestUri = requestUri,
                Result = new QueryResult<TRecord>
                {
                    Count = count,
                    Records = records
                },
            };
        }

        public QueryRecordsResponse<TRecord> BuildErrorResponse(ErrorResponse errors, string requestUri = "")
        {
            return new QueryRecordsResponse<TRecord>()
            {
                RequestUri = requestUri,
                Errors = errors,
                Result = new QueryResult<TRecord>
                {
                    Count = 0,
                    Records = new TRecord[] { },
                },
            };
        }
    }

    public class QueryResult<TRecord>
    {
        public long Count { get; set; } = 0;

        public TRecord[] Records { get; set; } = new TRecord[] { };
    }

    public class QueryResultBaseResponse
    {
        /// <summary>
        /// Id of the data.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// When the data was added.
        /// </summary>
        public DateTime AddedOn { get; set; } = DateTime.UtcNow;
    }
}
