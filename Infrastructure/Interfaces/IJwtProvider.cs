using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(string email, string userId, string role);
        string GeneratePasswordRecoveryToken(string email, string userId, string role);
        string GenerateEmailConfirmationToken(string email, string userId, string role);
        Task<TokenValidationResult?> ValidateTokenAsync(string token);
        Task<TokenValidationResult?> ValidateEmailTokenAsync(string token);
        Task<TokenValidationResult?> ValidatePasswordTokenAsync(string token);
    }
}