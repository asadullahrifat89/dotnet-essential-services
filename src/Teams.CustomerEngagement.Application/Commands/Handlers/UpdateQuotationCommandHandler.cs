using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using Identity.Application.Providers.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Teams.CustomerEngagement.Application.Commands.Validators;
using Teams.CustomerEngagement.Domain.Repositories.Interfaces;

namespace Teams.CustomerEngagement.Application.Commands.Handlers
{
    public class UpdateQuotationCommandHandler : IRequestHandler<UpdateQuotationCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<UpdateQuotationCommandHandler> _logger;
        private readonly UpdateQuotationCommandValidator _validator;
        private readonly IQuotationRepository _quotationRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public UpdateQuotationCommandHandler(
            ILogger<UpdateQuotationCommandHandler> logger,
            UpdateQuotationCommandValidator validator,
            IQuotationRepository QuotationRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _quotationRepository = QuotationRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(UpdateQuotationCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();

                var existingQuotation = await _quotationRepository.GetQuotation(command.QuotationId);

                var quotation = UpdateQuotationCommand.Initialize(command);

                var result = await _quotationRepository.UpdateQuotation(quotation);

                if (existingQuotation.QuoteStatus != result.QuoteStatus)
                {
                    // TODO: send email to quotation customer
                }

                return Response.BuildServiceResponse().BuildSuccessResponse(result, authCtx?.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildServiceResponse().BuildErrorResponse(ex.Message, _authenticationContextProvider.GetAuthenticationContext()?.RequestUri);
            }
        }

        #endregion
    }
}
