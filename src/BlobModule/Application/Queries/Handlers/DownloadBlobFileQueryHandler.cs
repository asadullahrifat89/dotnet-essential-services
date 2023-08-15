using MediatR;
using Microsoft.Extensions.Logging;
using BaseModule.Infrastructure.Extensions;
using BlobModule.Application.DTOs.Responses;
using BlobModule.Application.Queries.Validators;
using BlobModule.Domain.Repositories.Interfaces;
using IdentityModule.Application.Providers.Interfaces;

namespace BlobModule.Application.Queries.Handlers
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

                return await _blobFileRepository.DownloadBlobFile(request.FileId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BlobFileResponse();
            }
        }
    }
}
