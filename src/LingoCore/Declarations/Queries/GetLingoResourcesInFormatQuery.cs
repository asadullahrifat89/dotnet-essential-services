﻿using BaseCore.Models.Responses;
using LingoCore.Models.Entities;
using MediatR;

namespace LingoCore.Declarations.Queries
{
    public class GetLingoResourcesInFormatQuery : IRequest<QueryRecordResponse<LingoResource>>
    {
        public string AppId { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;
    }
}