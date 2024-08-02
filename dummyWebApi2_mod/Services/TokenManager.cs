using dummyWebApi2.Models.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace dummyWebApi2.Services
{
    public static class TokenManager
    {
        private static IConfiguration _configuration = null!;
        private static UserManager _userManager = new UserManager();

        public static void SetConfiguration(IConfiguration configuration)
        {
            if (_configuration == null) // we only need to set it once per instance
            {
                _configuration = configuration;
            }
        }

        public static string GenerateJwtToken(ApplicationUser user)
        {
            var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            var credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);
                        
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.PublicID.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.DateOfBirth, user.DateJoin.ToString())
            };

            var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:issuer"],
                    audience: _configuration["Jwt:audience"],
                    claims: claims,
                    expires: DateTime.Now.AddDays(30),
                    signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static bool ValidateJwtToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();

            var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));

            var validationParam = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidIssuer = _configuration["Jwt:issuer"],
                ValidAudience = _configuration["Jwt:audience"],
                IssuerSigningKey = secKey
            };

            try
            {
                var result = handler.ValidateToken(token, validationParam, out _);

                return result != null;
            }
            catch
            {
                return false;
            }
        }

        public static bool ValidateJwtTokenWithRole(string token, string role)
        {
            var handler = new JwtSecurityTokenHandler();

            var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));

            var validationParam = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidIssuer = _configuration["Jwt:issuer"],
                ValidAudience = _configuration["Jwt:audience"],
                IssuerSigningKey = secKey
            };

            try
            {
                var result = handler.ValidateToken(token, validationParam, out _);

                var userPublicID = result.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value.ToString();

                var userManager = _userManager.FindUserByPublicID(userPublicID).Result;
            
                if (userManager != null)
                {
                    return RoleManager.GetUserRoles(userManager.ID).Contains(role);
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
