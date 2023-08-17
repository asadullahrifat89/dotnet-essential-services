using Base.Application.DTOs.Responses;
using Email.Domain.Entities;
using MediatR;

namespace Email.Application.Queries
{
    public class GetEmailTemplateQuery : IRequest<QueryRecordResponse<EmailTemplate>>
    {
        public string TemplateId { get; set; } = string.Empty;
    }
}
