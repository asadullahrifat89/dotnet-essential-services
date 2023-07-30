using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Declarations.Services
{
    public interface IMongoDbService
    {
        Task<bool> InsertDocument<T>(T document);
    }
}
