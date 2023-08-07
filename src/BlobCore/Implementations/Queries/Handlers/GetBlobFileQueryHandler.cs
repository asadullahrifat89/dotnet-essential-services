using BaseCore.Models.Responses;
using BaseCore.Services;
using BlobCore.Declarations.Queries;
using BlobCore.Declarations.Repositories;
using BlobCore.Implementations.Queries.Validators;
using BlobCore.Models.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using BaseCore.Extensions;

namespace BlobCore.Implementations.Queries.Handlers
{
    public class GetBlobFileQueryHandler : IRequestHandler<GetBlobFileQuery, QueryRecordResponse<BlobFile>>
    {
        private readonly ILogger<GetBlobFileQueryHandler> _logger;
        private readonly GetBlobFileQueryValidator _validator;
        private readonly IBlobFileRepository _blobFileRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;

        public GetBlobFileQueryHandler(ILogger<GetBlobFileQueryHandler> logger,
            GetBlobFileQueryValidator validator,
            IBlobFileRepository blobFileRepository,
            IAuthenticationContextProvider authenticationContext)
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
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<BlobFile>().BuildErrorResponse(
                                       Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
