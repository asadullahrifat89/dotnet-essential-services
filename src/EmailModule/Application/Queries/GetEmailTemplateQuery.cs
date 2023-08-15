using BaseModule.Application.DTOs.Responses;
using EmailModule.Domain.Entities;
using MediatR;

namespace EmailModule.Application.Queries
{
    public class GetEmailTemplateQuery : IRequest<QueryRecordResponse<EmailTemplate>>
    {
        public string TemplateId { get; set; } = string.Empty;
    }
}
