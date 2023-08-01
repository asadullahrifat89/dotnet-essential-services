namespace IdentityCore.Contracts.Declarations.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(string userId, string[] userClaims);

        string ValidateJwtToken(string? token);
    }
}
