using BaseCore.Models.Responses;
using BaseCore.Services;
using BlobCore.Declarations.Commands;
using BlobCore.Declarations.Repositories;
using BlobCore.Models.Entities;
using BaseCore.Extensions;
using BlobCore.Declarations.Queries;
using MongoDB.Driver;
using BlobCore.Models.Responses;

namespace BlobCore.Implementations.Repositories
{
    public class BlobFileRepository : IBlobFileRepository
    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContext;

        #endregion

        #region Ctor

        public BlobFileRepository(IMongoDbService mongoDbService, IAuthenticationContextProvider authenticationContext)
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
                    ContentType = file.ContentType,
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

            return await _mongoDbService.Exists<BlobFile>(filter);
        }

        #endregion
    }
}
