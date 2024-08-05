using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Models.Data;

namespace UserService.Services.Security
{
    public static class JwtTokenManager
    {
        private static IConfiguration _configuration = null!;

        /// <summary>
        /// Allows JwtTokenManager to access application secrets
        /// </summary>
        public static void SetConfigurationReference(IConfiguration configuration)
        {
            if (_configuration == null)
            {
                _configuration = configuration;
            }
        }

        public static string GenerateUserToken(User user)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            var appCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.PublicID.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.DateOfBirth, user.DateJoin.ToString())
            };

            var tokenInstance = new JwtSecurityToken(
                    issuer: _configuration["Jwt:issuer"],
                    audience: _configuration["Jwt:audience"],
                    claims: userClaims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: appCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenInstance);
        }

        public static bool ValidateUserToken(string token)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));

            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParams = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidIssuer = _configuration["Jwt:issuer"],
                ValidAudience = _configuration["Jwt:audience"],
                IssuerSigningKey = secretKey
            };

            // token might be empty, damaged, changed, completely unrelated to the website
            try
            {
                var result = tokenHandler.ValidateToken(token, validationParams, out _);

                return result != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
