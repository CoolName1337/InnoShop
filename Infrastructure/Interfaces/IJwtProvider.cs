using Microsoft.IdentityModel.Tokens;
using UserService.DAL.Entities;

namespace Infrastructure.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
        string GenerateEmailConfirmationToken(User user);
        Task<TokenValidationResult?> ValidatePasswordTokenAsync(string token);
        Task<TokenValidationResult?> ValidateEmailTokenAsync(string token);
        string GeneratePasswordRecoveryToken(User user);
    }
}