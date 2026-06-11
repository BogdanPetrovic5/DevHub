using Backend.Dto;
using Backend.Interfaces.Authentication;
using Backend.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IAuthenticationService _authenticationService;
        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
            
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegistrationDto registrationDto)
        {
            AuthResponse authResponse = await _authenticationService.Register(registrationDto);
            if (!authResponse.Success)
                return BadRequest(authResponse);
            return Ok(authResponse);
        }
    }
}
