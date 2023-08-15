using BaseModule.Application.DTOs.Responses;
using EmailModule.Models.Entities;
using MediatR;

namespace EmailModule.Declarations.Queries
{
    public class GetEmailTemplateQuery : IRequest<QueryRecordResponse<EmailTemplate>>
    {
        public string TemplateId { get; set; } = string.Empty;
    }
}
