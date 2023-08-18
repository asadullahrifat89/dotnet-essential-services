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

        public static Quotation Initialize(AddQuotationCommand command)
        {
            var quotation = new Quotation()
            {
                Email = command.Email,
                Title = command.Title,
                Location = command.Location,
                ManPower = command.ManPower,
                Experience = command.Experience,
                EmploymentTypes = command.EmploymentTypes,
                SubmittedProductSearchCriterias = command.SubmittedProductSearchCriterias,
                TimeStamp = new Base.Domain.Entities.TimeStamp() { CreatedOn = DateTime.UtcNow } // as this is an anonymus submission no user can be tagged
            };

            return quotation;
        }
    }
}
