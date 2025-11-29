using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.DAL.Entities;

namespace Infrastructure
{
    public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
    {
        public async Task<TokenValidationResult?> ValidatePasswordTokenAsync(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                options.Value.SecretKeyForPasswordConfirmation));
            return await ValidateTokenAsync(token, key);
        }

        public async Task<TokenValidationResult?> ValidateEmailTokenAsync(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                options.Value.SecretKeyForEmailConfirmation));
            return await ValidateTokenAsync(token, key);
        }

        private async Task<TokenValidationResult?> ValidateTokenAsync(string token, SymmetricSecurityKey key)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key
            });
            return tokenValidationResult;
        }
        public string GenerateToken(User user)
        {
            List<Claim> claims = [
                new("userId", user.Id.ToString()),
                new(ClaimTypes.Name, user.Name),
                new(ClaimTypes.Role, user.Role.Name)
            ];

            foreach (var rp in user.Role.RolePermissions)
            {
                claims.Add(new("permission", rp.Permission.Name));
            }

            return GenerateToken(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey)),
                DateTime.UtcNow.AddHours(options.Value.ExpireHours),
                claims
                );
        }

        public string GenerateEmailConfirmationToken(User user)
        {
            var claims = new List<Claim>()
            {
                new("userEmail", user.Email),
                new("userId", user.Id.ToString())
            };

            return GenerateToken(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKeyForEmailConfirmation)),
                DateTime.UtcNow.AddMinutes(options.Value.ExpireMinutesForEmailConfirmation),
                claims
                );
        }
        public string GeneratePasswordRecoveryToken(User user)
        {
            var claims = new List<Claim>()
            {
                new("userEmail", user.Email),
                new("userId", user.Id.ToString())
            };

            return GenerateToken(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKeyForPasswordConfirmation)),
                DateTime.UtcNow.AddMinutes(options.Value.ExpireMinutesForPasswordConfirmation),
                claims
                );
        }
        private string GenerateToken(SymmetricSecurityKey key, DateTime expires, IEnumerable<Claim> claims)
        {
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: creds,
                expires: expires
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
