using Base.Application.DTOs.Responses;
using MediatR;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Application.Queries
{
    public class GetProjectsForProductIdQuery : IRequest<QueryRecordResponse<Project>>
    {
        public string ProductId { get; set; } = string.Empty;
    }
}
