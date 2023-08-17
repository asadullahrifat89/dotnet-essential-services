using MongoDB.Driver;
using Microsoft.AspNetCore.Http;
using Blob.Domain.Repositories.Interfaces;
using Base.Application.Providers.Interfaces;
using Blob.Domain.Entities;
using Base.Application.Extensions;
using Identity.Application.Providers.Interfaces;
using Identity.Application.Extensions;

namespace Blob.Infrastructure.Persistence
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

        public async Task<BlobFile> UploadBlobFile(IFormFile formFile)
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

            return blobFile;
        }

        public async Task<(BlobFile blobFile, byte[] bytes)> DownloadBlobFile(string fileId)
        {
            var blobFile = await _mongoDbContextProvider.FindById<BlobFile>(fileId);

            var bytes = await _mongoDbContextProvider.DownloadFileBytes(blobFile.BucketObjectId);

            return (blobFile, bytes);
        }

        public async Task<bool> BeAnExistingBlobFile(string fileId)
        {
            var filter = Builders<BlobFile>.Filter.Eq(x => x.Id, fileId);

            return await _mongoDbContextProvider.Exists(filter);
        }

        public async Task<BlobFile> GetBlobFile(string fileId)
        {
            var filter = Builders<BlobFile>.Filter.Eq(x => x.Id, fileId);

            var blobFile = await _mongoDbContextProvider.FindOne(filter);

            return blobFile;
        }

        #endregion
    }
}
