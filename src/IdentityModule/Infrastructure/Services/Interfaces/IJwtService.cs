namespace IdentityModule.Infrastructure.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(string userId, string[] claims);

        string ValidateJwtToken(string? token);
    }
}
