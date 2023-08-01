using IdentityCore.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Models.Requests
{
    public class PagedRequestBase<T> : IRequest<QueryRecordsResponse<T>>
    {
        /// <summary>
        /// Page index determines the page number of the query. Starts at zero.
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// Page size determines how many records to return from this query.
        /// </summary>
        public int PageSize { get; set; }
    }

}
