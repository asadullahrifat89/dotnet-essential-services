using Base.Infrastructure.Providers.Interfaces;
using Email.Domain.Entities;
using Email.Domain.Repositories.Interfaces;
using Identity.Application.Providers.Interfaces;
using MongoDB.Driver;


namespace Email.Infrastructure.Persistence
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbContextProvider;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public EmailTemplateRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProvider authenticationContext)
        {
            _mongoDbContextProvider = mongoDbService;
            _authenticationContextProvider = authenticationContext;
        }


        #endregion

        #region Methods

        public async Task<EmailTemplate> CreateEmailTemplate(EmailTemplate emailTemplate)
        {
            await _mongoDbContextProvider.InsertDocument(emailTemplate);

            return emailTemplate;
        }

        public async Task<EmailTemplate> GetEmailTemplate(string templateId)
        {   
            var filter = Builders<EmailTemplate>.Filter.Eq(x => x.Id, templateId);

            var emailTemplate = await _mongoDbContextProvider.FindOne(filter);

            return emailTemplate;
        }

        public async Task<EmailTemplate> UpdateEmailTemplate(EmailTemplate emailTemplate)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var update = Builders<EmailTemplate>.Update
                .Set(x => x.Name, emailTemplate.Name)
                .Set(x => x.Body, emailTemplate.Body)
                .Set(x => x.EmailBodyContentType, emailTemplate.EmailBodyContentType)
                .Set(x => x.Purpose, emailTemplate.Purpose)
                .Set(x => x.Tags, emailTemplate.Tags)
                .Set(x => x.TimeStamp.ModifiedOn, DateTime.UtcNow)
                .Set(x => x.TimeStamp.ModifiedBy, authCtx.User?.Id);

            await _mongoDbContextProvider.UpdateById(update: update, id: emailTemplate.Id);

            var updatedTemplate = await _mongoDbContextProvider.FindById<EmailTemplate>(emailTemplate.Id);

            return updatedTemplate;
        }

        public async Task<bool> BeAnExistingEmailTemplate(string templateName)
        {
            var filter = Builders<EmailTemplate>.Filter.Where(x => x.Name.ToLower().Equals(templateName.ToLower()));
            return await _mongoDbContextProvider.Exists(filter);
        }

        public async Task<bool> BeAnExistingEmailTemplateById(string templateId)
        {
            var filter = Builders<EmailTemplate>.Filter.Eq(x => x.Id, templateId);
            return await _mongoDbContextProvider.Exists(filter);
        }

        public async Task<EmailTemplate> GetEmailTemplateById(string templateId)
        {
            var filter = Builders<EmailTemplate>.Filter.Eq(x => x.Id, templateId);

            var emailTemplate = await _mongoDbContextProvider.FindOne(filter);

            return emailTemplate;
        }

        public async Task<EmailTemplate> GetEmailTemplateByPurpose(string purpose)
        {
            var filter = Builders<EmailTemplate>.Filter.Where(x => x.Purpose.ToLower().Equals(purpose.ToLower()));

            var emailTemplate = await _mongoDbContextProvider.FindOne(filter);

            return emailTemplate;
        }

        #endregion
    }
}