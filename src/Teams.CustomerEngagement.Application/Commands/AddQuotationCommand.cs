using Base.Application.DTOs.Responses;
using MediatR;
using Teams.ContentMangement.Domain.Entities;
using Teams.CustomerEngagement.Domain.Entities;

namespace Teams.CustomerEngagement.Application.Commands
{
    public class AddQuotationCommand : IRequest<ServiceResponse>
    {
        public string Email { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public int ManPower { get; set; }

        public int Experience { get; set; } = 0;

        public EmploymentType[] EmploymentTypes { get; set; } = new EmploymentType[] { };

        public SubmittedProductSearchCriteria[] SubmittedProductSearchCriterias { get; set; } = new SubmittedProductSearchCriteria[] { };
    }
}
