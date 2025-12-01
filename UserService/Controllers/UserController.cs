using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;
using UserService.Contracts.DTOs;
using UserService.Contracts.Interfaces;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(
        IUserService userService,
        IValidationService validationService,
        LinkGenerator linkGenerator) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUser, CancellationToken ct)
        {
            await validationService.ValidateAsync(registerUser);

            var userDto = await userService.Register(registerUser, ct);

            await SendConfirmation(userDto.Email, ct);

            return Created();
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO loginUser, CancellationToken ct)
        {
            await validationService.ValidateAsync(loginUser);

            var token = await userService.Login(loginUser, ct);

            HttpContext.Response.Cookies.Append("nyam-nyam", token);

            return Ok();
        }

        [HttpPost("send-confirmation")]
        public async Task<IActionResult> SendConfirmation(string email, CancellationToken ct)
        {
            var link = linkGenerator.GetUriByAction(
                httpContext: HttpContext,
                action: "ConfirmEmail",
                controller: "User"
                );

            await userService.SendConfirmationEmail(link, email, ct);

            return Ok("Confirmation email sent");
        }

        [HttpGet("send-recovery")]
        public async Task<IActionResult> SendRecovery(string toEmail, CancellationToken ct)
        {
            var link = linkGenerator.GetUriByAction(
                httpContext: HttpContext,
                action: "ResetPassword",
                controller: "User"
                );
            await userService.SendRecoveryEmail(link, toEmail, ct);

            return Ok();
        }
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string token, CancellationToken ct)
        {
            await userService.ConfirmEmail(token, ct);

            return Ok("Email confirmed");

        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPassword, CancellationToken ct)
        {
            await validationService.ValidateAsync(resetPassword);

            await userService.ResetPassword(resetPassword, ct);

            return Ok("Password reset");
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var users = await userService.GetAllAsync(ct);

            return Ok(users);
        }
    }
}
