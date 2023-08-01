namespace IdentityCore.Models.Responses
{
    public class QueryRecordResponse<TRecord>
    {
        public ErrorResponse Errors { get; set; } = new ErrorResponse();

        public TRecord? Result { get; set; }

        public bool IsSuccess => Errors.Errors is null || Errors.Errors.Count() == 0;

        public QueryRecordResponse<TRecord> BuildSuccessResponse(TRecord result)
        {
            return new QueryRecordResponse<TRecord>()
            {
                Result = result,
            };
        }

        public QueryRecordResponse<TRecord> BuildErrorResponse(ErrorResponse errors)
        {
            return new QueryRecordResponse<TRecord>()
            {
                Errors = errors,
            };
        }
    }
}
