using IdentityCore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Declarations.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(string userId, string[] userClaims);

        string ValidateJwtToken(string? token);
    }
}
