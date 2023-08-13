using BaseCore.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsCore.Declarations.Commands
{
    public class SubmitQuoteRequestCommand : IRequest<ServiceResponse>
    {
        public string Email { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public int NumberOfResources { get; set; }

        public string[] QueriedSearchCriteriaIds { get; set; } = new string[0];

        public string[] RecommendedProductIds { get; set; } = new string[0];
    }
}
