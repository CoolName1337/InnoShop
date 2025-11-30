using AutoMapper;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using UserService.BLL.Exceptions;
using UserService.Contracts.DTOs;
using UserService.Contracts.Interfaces;
using UserService.DAL.Entities;
using UserService.DAL.Interfaces;

namespace UserService.BLL.Services
{
    public class UserService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IPasswordHasher passwordHasher,
        IEmailService emailService,
        IJwtProvider jwtProvider,
        IMapper mapper,
        ITokenProvider tokenProvider,
        ILogger<UserService> logger) : IUserService
    {
        public async Task<string> Login(LoginUserDTO loginUser, CancellationToken ct)
        {
            var user = await userRepository.GetByEmailAsync(loginUser.Email, ct);
            if (user == null) throw new FailedToLoginException("Username or email non correct");

            logger.LogInformation("[UserService]Login TRY {Email}", user.Email);

            if (!user.IsEmailConfirmed) throw new FailedToLoginException("Email not vailidated");

            var result = passwordHasher.Verify(loginUser.Password, user.PasswordHash);

            if (!result) throw new FailedToLoginException("Invalid credentials");

            var tokenString = jwtProvider.GenerateToken(user.Email, user.Id.ToString(), user.Role.Name);
            tokenProvider.Token = tokenString;

            logger.LogInformation("[UserService]Login CORRECT {Email}", user.Email);
            return tokenString;
        }

        public async Task<UserDTO> Register(RegisterUserDTO registerUser, CancellationToken ct)
        {
            logger.LogInformation("[UserService]Register TRY {Email}", registerUser.Email);

            var userWithEmail = await userRepository.GetByEmailAsync(registerUser.Email, ct);
            if (userWithEmail != null) throw new FailedToRegisterException($"User with email:{userWithEmail.Email} is already exist");

            var hashedPassword = passwordHasher.Generate(registerUser.Password);

            var user = mapper.Map<User>(registerUser);

            user.RoleId = await roleRepository.GetIdByName("User");
            user.PasswordHash = hashedPassword;
            user.IsActive = true;
            user.DateOfCreating = DateOnly.FromDateTime(DateTime.Now);

            user = await userRepository.CreateAsync(user, ct);

            logger.LogInformation("[UserService]Register CORRECT {Email}", registerUser.Email);

            return mapper.Map<UserDTO>(user);
        }

        public async Task SendConfirmationEmail(string link, string email, CancellationToken ct)
        {
            logger.LogInformation("[UserService]SendConfirmationEmail TRY {email}", email);

            var user = await userRepository.GetByEmailAsync(email, ct);

            if (user == null) throw new NotFoundUserException("Can't to send confirmation message because user not found");

            if (user.IsEmailConfirmed) throw new FailedToSendEmail("Email is already confirmed");

            var token = jwtProvider.GenerateEmailConfirmationToken(user.Email, user.Id.ToString(), user.Role.Name);

            link += $"?token={token}";

            await emailService.SendConfirmationLinkAsync(user.Email, link);

            logger.LogInformation("[UserService]SendConfirmationEmail CORRECT {email}", email);
        }

        public async Task ConfirmEmail(string token, CancellationToken ct)
        {
            logger.LogInformation("[UserService]ConfirmEmail TRY");

            var tokenRes = await jwtProvider.ValidateEmailTokenAsync(token)
                ?? throw new FailedToValidate("Invalid or expired token");

            if (!tokenRes.IsValid)
                throw tokenRes.Exception;

            var userIdClaim = tokenRes.ClaimsIdentity.FindFirst("userId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                var user = await userRepository.GetByIdAsync(userId, ct);

                if (user == null)
                    throw new NotFoundUserException("Token non correct");

                user.IsEmailConfirmed = true;
                await userRepository.UpdateAsync(user, ct);
            }
            else
            {
                throw new FailedToValidate("UserId claim in token not found");
            }

            logger.LogInformation("[UserService]ConfirmEmail CORRECT UserId:{userId}", userId);
        }

        public async Task SendRecoveryEmail(string link, string email, CancellationToken ct)
        {
            logger.LogInformation("[UserService]SendRecoveryEmail TRY {email}", email);

            var user = await userRepository.GetByEmailAsync(email, ct);

            if (user == null) throw new NotFoundUserException("Can't to send recovery message because user not found");

            var token = jwtProvider.GeneratePasswordRecoveryToken(user.Email, user.Id.ToString(), user.Role.Name);

            link += $"?token={token}";

            await emailService.SendRecoveryLinkAsync(user.Email, link);

            logger.LogInformation("[UserService]SendRecoveryEmail CORRECT {email}", email);
        }

        public async Task ResetPassword(ResetPasswordDTO resetPassword, CancellationToken ct)
        {
            logger.LogInformation("[UserService]RestorePassword TRY");

            var tokenRes = await jwtProvider.ValidatePasswordTokenAsync(resetPassword.Token)
                ?? throw new FailedToValidate("Invalid or expired token");

            if (!tokenRes.IsValid)
                throw tokenRes.Exception;

            var userIdClaim = tokenRes.ClaimsIdentity.FindFirst("userId");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                var user = await userRepository.GetByIdAsync(userId, ct);

                if (user == null)
                    throw new NotFoundUserException("Token non correct");

                user.PasswordHash = passwordHasher.Generate(resetPassword.NewPassword);
                await userRepository.UpdateAsync(user, ct);
            }
            else
            {
                throw new FailedToValidate("UserId claim in token not found");
            }

            logger.LogInformation("[UserService]RestorePassword CORRECT UserId:{userId}", userId);
        }

        public async Task<List<UserDTO>> GetAllAsync(CancellationToken ct)
        {
            var users = await userRepository.GetAllAsync(ct);

            return mapper.Map<List<UserDTO>>(users);
        }
    }
}
