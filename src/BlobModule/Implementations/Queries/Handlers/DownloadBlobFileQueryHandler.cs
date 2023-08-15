using MediatR;
using Microsoft.Extensions.Logging;
using BlobModule.Declarations.Repositories;
using BaseModule.Services;
using BlobModule.Models.Responses;
using BlobModule.Declarations.Queries;
using BlobModule.Implementations.Queries.Validators;
using BaseModule.Extensions;

namespace BlobModule.Implementations.Queries.Handlers
{
    public class DownloadBlobFileQueryHandler : IRequestHandler<DownloadBlobFileQuery, BlobFileResponse>
    {
        private readonly ILogger<DownloadBlobFileQueryHandler> _logger;
        private readonly DownloadBlobFileQueryValidator _validator;
        private readonly IAuthenticationContextProvider _authenticationContext;
        private readonly IBlobFileRepository _blobFileRepository;

        public DownloadBlobFileQueryHandler(ILogger<DownloadBlobFileQueryHandler> logger, DownloadBlobFileQueryValidator validator, IAuthenticationContextProvider authenticationContext, IBlobFileRepository blobFileRepository)
        {
            _logger = logger;
            _validator = validator;
            _authenticationContext = authenticationContext;
            _blobFileRepository = blobFileRepository;
        }

        public async Task<BlobFileResponse> Handle(DownloadBlobFileQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _blobFileRepository.DownloadBlobFile(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BlobFileResponse();
            }
        }
    }
}
