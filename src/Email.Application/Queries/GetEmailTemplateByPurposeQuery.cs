using Base.Application.DTOs.Responses;
using Email.Domain.Entities;
using MediatR;

namespace Email.Application.Queries
{
    public class GetEmailTemplateByPurposeQuery : IRequest<QueryRecordResponse<EmailTemplate>>
    {
        public string Purpose { get; set; } = string.Empty;
    }
}
