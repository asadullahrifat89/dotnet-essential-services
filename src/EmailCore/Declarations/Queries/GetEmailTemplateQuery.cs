using BaseCore.Models.Responses;
using EmailCore.Models.Entities;
using MediatR;

namespace EmailCore.Declarations.Queries
{
    public class GetEmailTemplateQuery : IRequest<QueryRecordResponse<EmailTemplate>>
    {
        public string TemplateId { get; set; } = string.Empty;
    }
}
