namespace IdentityCore.Models.Responses
{
    public class QueryRecordsResponse<TRecord>
    {
        public ErrorResponse Errors { get; set; } = new ErrorResponse();

        public QueryResult<TRecord> Result { get; set; } = new QueryResult<TRecord>();

        public bool IsSuccess => Errors.Errors is null || Errors.Errors.Count() == 0;

        public QueryRecordsResponse<TRecord> BuildSuccessResponse(
            long count,
            TRecord[] records)
        {
            return new QueryRecordsResponse<TRecord>()
            {
                Result = new QueryResult<TRecord>
                {
                    Count = count,
                    Records = records
                },
            };
        }

        public QueryRecordsResponse<TRecord> BuildErrorResponse(ErrorResponse errors)
        {
            return new QueryRecordsResponse<TRecord>()
            {
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
