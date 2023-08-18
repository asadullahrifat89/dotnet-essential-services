using Base.Application.DTOs.Responses;
using MediatR;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Application.Queries
{
    public class GetProjectQuery : IRequest<QueryRecordResponse<Project>>
    {
        public string ProjectId { get; set; } = string.Empty;
    }
}
