namespace BaseModule.Domain.DTOs.Responses
{
    public static class Response
    {
        public static ServiceResponse BuildServiceResponse()
        {
            return new ServiceResponse();
        }

        public static ErrorResponse BuildErrorResponse()
        {
            return new ErrorResponse();
        }

        public static QueryRecordResponse<T> BuildQueryRecordResponse<T>()
        {
            return new QueryRecordResponse<T>();
        }

        public static QueryRecordsResponse<T> BuildQueryRecordsResponse<T>()
        {
            return new QueryRecordsResponse<T>();
        }
    }
}
