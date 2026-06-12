using Backend.Dto;
using Backend.Interfaces.Authentication;
using Backend.Interfaces.Security;
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
        private ICookieService _cookieService;
        public AuthenticationController(IAuthenticationService authenticationService, ICookieService cookieService)
        {
            _authenticationService = authenticationService;
            _cookieService = cookieService;
            
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegistrationDto registrationDto)
        {
            AuthResponse authResponse = await _authenticationService.Register(registrationDto);
            if (!authResponse.Success)
                return BadRequest(authResponse);

            _cookieService.AppendAuthCookies(Response, authResponse.AccessToken, authResponse.RefreshToken);

            return Ok(authResponse);
        }
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(LoginDto loginDto)
        {
            AuthResponse authResponse = await _authenticationService.Login(loginDto);
            if (!authResponse.Success)
            {
                return Unauthorized(authResponse);
            }

            _cookieService.AppendAuthCookies(Response, authResponse.AccessToken, authResponse.RefreshToken);

            return Ok(authResponse);
        }
    }
}
