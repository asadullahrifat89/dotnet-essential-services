using MongoDB.Driver;
using BaseModule.Infrastructure.Extensions;
using BlobModule.Domain.Entities;
using BlobModule.Domain.Repositories.Interfaces;
using BlobModule.Application.DTOs.Responses;
using BaseModule.Application.DTOs.Responses;
using BlobModule.Application.Queries;
using BlobModule.Application.Commands;
using IdentityModule.Infrastructure.Services.Interfaces;
using IdentityModule.Infrastructure.Extensions;
using BaseModule.Infrastructure.Providers.Interfaces;

namespace BlobModule.Domain.Repositories.Implementations
{
    public class BlobFileRepository : IBlobFileRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbService;
        private readonly IAuthenticationContextProviderService _authenticationContext;

        #endregion

        #region Ctor

        public BlobFileRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProviderService authenticationContext)
        {
            _mongoDbService = mongoDbService;
            _authenticationContext = authenticationContext;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> UploadBlobFile(UploadBlobFileCommand command)
        {
            var authctx = _authenticationContext.GetAuthenticationContext();

            var file = command.FormFile;
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];

            var contentType = ContentTypeExtensions.GetContentType(extension);

            var blobFile = new BlobFile();

            using (var stream = file.OpenReadStream())
            {
                var bucketId = await _mongoDbService.UploadFileStream(file.Name, stream);

                blobFile = new BlobFile()
                {
                    Name = file.FileName,
                    BucketObjectId = bucketId,
                    Extension = extension,
                    TimeStamp = authctx.BuildCreatedByTimeStamp(),
                    ContentType = contentType,
                };

                await _mongoDbService.InsertDocument(blobFile);
            }

            return Response.BuildServiceResponse().BuildSuccessResponse(blobFile, authctx.RequestUri);
        }

        public async Task<BlobFileResponse> DownloadBlobFile(DownloadBlobFileQuery query)
        {
            var blobFile = await _mongoDbService.FindById<BlobFile>(query.FileId);

            var bytes = await _mongoDbService.DownloadFileBytes(blobFile.BucketObjectId);

            return BlobFileResponse.Initialize(blobFile, bytes);
        }

        public async Task<bool> BeAnExistingBlobFile(string fileId)
        {
            var filter = Builders<BlobFile>.Filter.Eq(x => x.Id, fileId);

            return await _mongoDbService.Exists(filter);
        }

        public async Task<QueryRecordResponse<BlobFile>> GetBlobFile(GetBlobFileQuery query)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var filter = Builders<BlobFile>.Filter.Eq(x => x.Id, query.FileId);

            var blobFile = await _mongoDbService.FindOne(filter);

            return Response.BuildQueryRecordResponse<BlobFile>().BuildSuccessResponse(blobFile, authCtx?.RequestUri);
        }

        #endregion
    }
}
