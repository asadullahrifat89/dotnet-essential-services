using BaseModule.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BaseModule.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(string userId, string[] userClaimNames)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"];

            var lifeTime = DateTime.UtcNow.AddSeconds(Convert.ToInt32(_configuration["Jwt:Lifetime"]));

            var keyBytes = Encoding.ASCII.GetBytes(key);

            var claims = new List<Claim>
            {
                new Claim("Id", userId)
            };

            var claim = new Claim("Permissions", userClaimNames != null && userClaimNames.Any() ? string.Join(",", userClaimNames) : "");
            claims.Add(claim);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = lifeTime,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }

        public string ValidateJwtToken(string? token)
        {
            if (token.IsNullOrBlank())
                return string.Empty;

            var tokenHandler = new JwtSecurityTokenHandler();

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"];
            var keyBytes = Encoding.ASCII.GetBytes(key);

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero, // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)

            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == "Id").Value;

            // return user id from JWT token if validation successful
            return userId;
        }
    }
}
