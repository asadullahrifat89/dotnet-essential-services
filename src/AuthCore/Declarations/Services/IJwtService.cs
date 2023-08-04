namespace BaseCore.Declarations.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(string userId, string[] userClaims);

        string ValidateJwtToken(string? token);
    }
}
