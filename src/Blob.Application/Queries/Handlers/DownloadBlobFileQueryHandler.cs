using MediatR;
using Microsoft.Extensions.Logging;
using Blob.Application.Queries.Validators;
using Blob.Application.DTOs.Responses;
using Identity.Application.Providers.Interfaces;
using Blob.Domain.Repositories.Interfaces;
using Base.Application.Extensions;

namespace Blob.Application.Queries.Handlers
{
    public class DownloadBlobFileQueryHandler : IRequestHandler<DownloadBlobFileQuery, BlobFileResponse>
    {
        private readonly ILogger<DownloadBlobFileQueryHandler> _logger;
        private readonly DownloadBlobFileQueryValidator _validator;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;
        private readonly IBlobFileRepository _blobFileRepository;

        public DownloadBlobFileQueryHandler(ILogger<DownloadBlobFileQueryHandler> logger, DownloadBlobFileQueryValidator validator, IAuthenticationContextProvider authenticationContext, IBlobFileRepository blobFileRepository)
        {
            _logger = logger;
            _validator = validator;
            _authenticationContextProvider = authenticationContext;
            _blobFileRepository = blobFileRepository;
        }

        public async Task<BlobFileResponse> Handle(DownloadBlobFileQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var result = await _blobFileRepository.DownloadBlobFile(request.FileId);

                var blobResponse = BlobFileResponse.Initialize(result.blobFile, result.bytes);

                return blobResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BlobFileResponse();
            }
        }
    }
}
