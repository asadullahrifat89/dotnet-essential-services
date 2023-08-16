using MongoDB.Driver;
using BaseModule.Infrastructure.Extensions;
using BlobModule.Domain.Entities;
using BlobModule.Application.DTOs.Responses;
using BaseModule.Application.DTOs.Responses;
using IdentityModule.Infrastructure.Extensions;
using BaseModule.Application.Providers.Interfaces;
using BlobModule.Domain.Repositories.Interfaces;
using IdentityModule.Application.Providers.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BlobModule.Infrastructure.Persistence
{
    public class BlobFileRepository : IBlobFileRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbContextProvider;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public BlobFileRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProvider authenticationContext)
        {
            _mongoDbContextProvider = mongoDbService;
            _authenticationContextProvider = authenticationContext;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> UploadBlobFile(IFormFile formFile)
        {
            var authctx = _authenticationContextProvider.GetAuthenticationContext();

            var file = formFile;
            var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];

            var contentType = ContentTypeExtensions.GetContentType(extension);

            var blobFile = new BlobFile();

            using (var stream = file.OpenReadStream())
            {
                var bucketId = await _mongoDbContextProvider.UploadFileStream(file.Name, stream);

                blobFile = new BlobFile()
                {
                    Name = file.FileName,
                    BucketObjectId = bucketId,
                    Extension = extension,
                    TimeStamp = authctx.BuildCreatedByTimeStamp(),
                    ContentType = contentType,
                };

                await _mongoDbContextProvider.InsertDocument(blobFile);
            }

            return Response.BuildServiceResponse().BuildSuccessResponse(blobFile, authctx.RequestUri);
        }

        public async Task<BlobFileResponse> DownloadBlobFile(string fileId)
        {
            var blobFile = await _mongoDbContextProvider.FindById<BlobFile>(fileId);

            var bytes = await _mongoDbContextProvider.DownloadFileBytes(blobFile.BucketObjectId);

            return BlobFileResponse.Initialize(blobFile, bytes);
        }

        public async Task<bool> BeAnExistingBlobFile(string fileId)
        {
            var filter = Builders<BlobFile>.Filter.Eq(x => x.Id, fileId);

            return await _mongoDbContextProvider.Exists(filter);
        }

        public async Task<QueryRecordResponse<BlobFile>> GetBlobFile(string fileId)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var filter = Builders<BlobFile>.Filter.Eq(x => x.Id, fileId);

            var blobFile = await _mongoDbContextProvider.FindOne(filter);

            return Response.BuildQueryRecordResponse<BlobFile>().BuildSuccessResponse(blobFile, authCtx?.RequestUri);
        }

        #endregion
    }
}
