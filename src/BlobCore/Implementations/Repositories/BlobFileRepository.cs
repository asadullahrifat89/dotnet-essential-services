using BaseCore.Models.Responses;
using BaseCore.Services;
using BlobCore.Declarations.Commands;
using BlobCore.Declarations.Repositories;
using BlobCore.Models.Entities;
using BaseCore.Extensions;

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
                };

                await _mongoDbService.InsertDocument(blobFile);
            }

            return Response.BuildServiceResponse().BuildSuccessResponse(blobFile, authctx.RequestUri);
        }

        #endregion
    }
}
