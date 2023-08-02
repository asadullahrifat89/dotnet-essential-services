using FluentValidation;
using IdentityCore.Contracts.Declarations.Queries;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Contracts.Implementations.Queries.Validators;
using IdentityCore.Extensions;
using IdentityCore.Models.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityCore.Contracts.Implementations.Queries.Handlers
{
    public class GetEndPointsQueryHandler : IRequestHandler<GetEndPointsQuery, string[]>
    {
        private readonly ILogger<GetUserQueryHandler> _logger;
        private readonly GetUserQueryValidator _validator;
        private readonly IUserRepository _userRepository;


        public GetEndPointsQueryHandler(
            ILogger<GetUserQueryHandler> logger,
            GetUserQueryValidator validator,
            IUserRepository userRepository)
        {
            _logger = logger;
            _validator = validator;
            _userRepository = userRepository;

        }


        public async Task<string[]> Handle(GetEndPointsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _userRepository.GetEndpointList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting endpoints");
                throw;
            }
        }
    }
   
}

