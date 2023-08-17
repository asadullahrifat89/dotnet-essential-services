using MediatR;
using Microsoft.Extensions.Logging;
using Blob.Application.Commands.Validators;
using Base.Application.DTOs.Responses;
using Blob.Domain.Repositories.Interfaces;
using Identity.Application.Providers.Interfaces;
using Base.Application.Extensions;

namespace Blob.Application.Commands.Handlers
{
    public class UploadBlobFileCommandHandler : IRequestHandler<UploadBlobFileCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<UploadBlobFileCommandHandler> _logger;
        private readonly UploadBlobFileCommandValidator _validator;
        private readonly IBlobFileRepository _blobFileRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public UploadBlobFileCommandHandler(
            ILogger<UploadBlobFileCommandHandler> logger,
            UploadBlobFileCommandValidator validator,
            IBlobFileRepository blobFileRepository,
            IAuthenticationContextProvider authenticationContextProvider)
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

                var authctx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _blobFileRepository.UploadBlobFile(request.FormFile);

                return Response.BuildServiceResponse().BuildSuccessResponse(result, authctx?.RequestUri);
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
