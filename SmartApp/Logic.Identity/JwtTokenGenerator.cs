using Microsoft.IdentityModel.Tokens;
using Shared.Models.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Logic.Identity
{
    public class JwtTokenGenerator
    {
        private readonly SecurityData _jwtData;
        public JwtTokenGenerator(SecurityData jwtData)
        {
            _jwtData = jwtData; 
        }

        public (string jwtToken, string refreshToken) GenerateToken(List<Claim> claims, int expireInDays)
        {
            return (GenerateJwtToken(claims, expireInDays), GenerateRefreshToken());
        }

        public ClaimsPrincipal GetPrincipalFromJwt(string jwt)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtData.Key));
           
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtData.Issuer,
                ValidAudience = _jwtData.Issuer,
                IssuerSigningKey = securityKey
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(jwt, tokenValidationParameters, out SecurityToken securityToken);

            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }

        public string GenerateJwtToken(List<Claim> claims, int expireInDays)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtData.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(_jwtData.Issuer,
             _jwtData.Issuer,
             claims,
             expires: DateTime.Now.AddDays(expireInDays),
             signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}

