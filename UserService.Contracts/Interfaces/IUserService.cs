using UserService.Contracts.DTOs;

namespace UserService.Contracts.Interfaces
{
    public interface IUserService
    {
        public Task<UserDTO> Register(RegisterUserDTO registerUser, CancellationToken ct);
        public Task<string> Login(LoginUserDTO loginUser, CancellationToken ct);
        Task ConfirmEmail(string token, CancellationToken ct);
        Task SendConfirmationEmail(string link, string email, CancellationToken ct);
        Task ResetPassword(ResetPasswordDTO resetPassword, CancellationToken ct);
        Task SendRecoveryEmail(string link, string email, CancellationToken ct);
        Task<List<UserDTO>> GetAllAsync(CancellationToken ct);
    }
}
