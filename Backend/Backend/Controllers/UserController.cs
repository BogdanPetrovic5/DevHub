using Backend.Dto.User;
using Backend.Exceptions;
using Backend.Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpGet("user/{username}")]
        public async Task<ActionResult<ProfileDto>> GetUser(string username)
        {
            var userIdClaims =   User.FindFirst(ClaimTypes.NameIdentifier);
            Guid? userId = userIdClaims != null ? Guid.Parse(userIdClaims.Value) : null;

            try
            {
                ProfileDto profile = await _userService.GetProfile(username);
                return Ok(profile);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
