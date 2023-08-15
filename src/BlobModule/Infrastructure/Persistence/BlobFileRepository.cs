using MongoDB.Driver;
using BaseModule.Infrastructure.Extensions;
using BlobModule.Domain.Entities;
using BlobModule.Application.DTOs.Responses;
using BaseModule.Application.DTOs.Responses;
using IdentityModule.Infrastructure.Extensions;
using BaseModule.Application.Providers.Interfaces;
using BlobModule.Domain.Repositories.Interfaces;
using IdentityModule.Application.Providers.Interfaces;
using BlobModule.Application.Queries;
using BlobModule.Application.Commands;
using Microsoft.AspNetCore.Http;

namespace BlobModule.Infrastructure.Persistence
{
    public class BlobFileRepository : IBlobFileRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContext;

        #endregion

        #region Ctor

        public BlobFileRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProvider authenticationContext)
        {
            _mongoDbService = mongoDbService;
            _authenticationContext = authenticationContext;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> UploadBlobFile(IFormFile formFile)
        {
            var authctx = _authenticationContext.GetAuthenticationContext();

            var file = formFile;
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

        public async Task<BlobFileResponse> DownloadBlobFile(string fileId)
        {
            var blobFile = await _mongoDbService.FindById<BlobFile>(fileId);

            var bytes = await _mongoDbService.DownloadFileBytes(blobFile.BucketObjectId);

            return BlobFileResponse.Initialize(blobFile, bytes);
        }

        public async Task<bool> BeAnExistingBlobFile(string fileId)
        {
            var filter = Builders<BlobFile>.Filter.Eq(x => x.Id, fileId);

            return await _mongoDbService.Exists(filter);
        }

        public async Task<QueryRecordResponse<BlobFile>> GetBlobFile(string fileId)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var filter = Builders<BlobFile>.Filter.Eq(x => x.Id, fileId);

            var blobFile = await _mongoDbService.FindOne(filter);

            return Response.BuildQueryRecordResponse<BlobFile>().BuildSuccessResponse(blobFile, authCtx?.RequestUri);
        }

        #endregion
    }
}
