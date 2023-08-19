using MediatR;
using Microsoft.Extensions.Logging;
using Blob.Application.Queries.Validators;
using Base.Application.DTOs.Responses;
using Blob.Domain.Repositories.Interfaces;
using Identity.Application.Providers.Interfaces;
using Blob.Domain.Entities;
using Base.Application.Extensions;

namespace Blob.Application.Queries.Handlers
{
    public class GetBlobFileQueryHandler : IRequestHandler<GetBlobFileQuery, QueryRecordResponse<BlobFile>>
    {
        private readonly ILogger<GetBlobFileQueryHandler> _logger;
        private readonly GetBlobFileQueryValidator _validator;
        private readonly IBlobFileRepository _blobFileRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        public GetBlobFileQueryHandler(ILogger<GetBlobFileQueryHandler> logger,
            GetBlobFileQueryValidator validator,
            IBlobFileRepository blobFileRepository,
            IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _blobFileRepository = blobFileRepository;
            _authenticationContextProvider = authenticationContext;
        }


        public async Task<QueryRecordResponse<BlobFile>> Handle(GetBlobFileQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _blobFileRepository.GetBlobFile(request.FileId);

                return Response.BuildQueryRecordResponse<BlobFile>().BuildSuccessResponse(result, authCtx.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<BlobFile>().BuildErrorResponse(
                                       Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
