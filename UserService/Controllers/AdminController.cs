using Infrastructure;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Contracts.DTOs;
using UserService.Contracts.Interfaces;

namespace UserService.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminOnly")]
    public class AdminController(IAdminService adminService) : ControllerBase
    {
        [HttpGet("User")]
        public async Task<IActionResult> Get([FromQuery]bool includeInactive, CancellationToken ct)
        {
            var users = await adminService.GetAllAsync(includeInactive, ct);

            return Ok(users);
        }

        [HttpGet("User/{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken ct)
        {
            var user = await adminService.GetByIdAsync(id, ct);

            return Ok(user);
        }

        [HttpPatch("User")]
        public async Task<IActionResult> Patch([FromBody] PatchUserDTO patchUser, CancellationToken ct)
        {
            var user = await adminService.UpdateUserAsync(patchUser, ct);
            
            return Ok(user);
        }

        [HttpDelete("User/{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            await adminService.DeleteUserAsync(id, ct);

            return Ok();
        }
    }
}
