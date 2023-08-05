using BaseCore.Models.Entities;
using BaseCore.Models.Responses;
using BaseCore.Services;
using BlobCore.Declarations.Commands;
using BlobCore.Declarations.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task<ServiceResponse> UploadBlobFile(UploadBlobFileCommand command)
        {
            throw new NotImplementedException();
        } 

        #endregion
    }
}
