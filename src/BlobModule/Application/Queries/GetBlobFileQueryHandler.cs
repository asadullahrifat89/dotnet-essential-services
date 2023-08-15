using MediatR;
using Microsoft.Extensions.Logging;
using BaseModule.Infrastructure.Extensions;
using BlobModule.Domain.Entities;
using BlobModule.Domain.Repositories.Interfaces;
using BaseModule.Infrastructure.Services.Interfaces;
using BaseModule.Application.DTOs.Responses;

namespace BlobModule.Application.Queries
{
    public class GetBlobFileQueryHandler : IRequestHandler<GetBlobFileQuery, QueryRecordResponse<BlobFile>>
    {
        private readonly ILogger<GetBlobFileQueryHandler> _logger;
        private readonly GetBlobFileQueryValidator _validator;
        private readonly IBlobFileRepository _blobFileRepository;
        private readonly IAuthenticationContextProviderService _authenticationContext;

        public GetBlobFileQueryHandler(ILogger<GetBlobFileQueryHandler> logger,
            GetBlobFileQueryValidator validator,
            IBlobFileRepository blobFileRepository,
            IAuthenticationContextProviderService authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _blobFileRepository = blobFileRepository;
            _authenticationContext = authenticationContext;
        }


        public async Task<QueryRecordResponse<BlobFile>> Handle(GetBlobFileQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _blobFileRepository.GetBlobFile(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<BlobFile>().BuildErrorResponse(
                                       Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
