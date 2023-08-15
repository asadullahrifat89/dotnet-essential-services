using MediatR;
using Microsoft.Extensions.Logging;
using BaseModule.Infrastructure.Extensions;
using BaseModule.Application.DTOs.Responses;
using BlobModule.Application.Commands.Validators;
using IdentityModule.Infrastructure.Services.Interfaces;
using BlobModule.Domain.Interfaces;

namespace BlobModule.Application.Commands.Handlers
{
    public class UploadBlobFileCommandHandler : IRequestHandler<UploadBlobFileCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<UploadBlobFileCommandHandler> _logger;
        private readonly UploadBlobFileCommandValidator _validator;
        private readonly IBlobFileRepository _blobFileRepository;
        private readonly IAuthenticationContextProviderService _authenticationContextProvider;

        #endregion

        #region Ctor

        public UploadBlobFileCommandHandler(
            ILogger<UploadBlobFileCommandHandler> logger,
            UploadBlobFileCommandValidator validator,
            IBlobFileRepository blobFileRepository,
            IAuthenticationContextProviderService authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _blobFileRepository = blobFileRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(UploadBlobFileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _blobFileRepository.UploadBlobFile(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildServiceResponse().BuildErrorResponse(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri);
            }
        }

        #endregion
    }
}
