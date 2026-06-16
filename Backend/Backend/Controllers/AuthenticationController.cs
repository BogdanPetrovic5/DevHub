using Backend.Dto;
using Backend.Interfaces.Authentication;
using Backend.Interfaces.Security;
using Backend.Models;
using Backend.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponse>> Refresh()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if(refreshToken == null)
            {
                return Unauthorized(new AuthResponse { Success = false, Message = "Refresh token is missing." });
            }

            AuthResponse authResponse = await _authenticationService.Refresh(refreshToken);
            if (!authResponse.Success) { 
               return Unauthorized(authResponse);
            }
            _cookieService.AppendAuthCookies(Response, authResponse.AccessToken, authResponse.RefreshToken, authResponse.RememberMe);
            return Ok(authResponse);

        }
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(RegistrationDto registrationDto)
        {
            AuthResponse authResponse = await _authenticationService.Register(registrationDto);
            if (!authResponse.Success)
                return BadRequest(authResponse);

            _cookieService.AppendAuthCookies(Response, authResponse.AccessToken, authResponse.RefreshToken, authResponse.RememberMe);

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

            _cookieService.AppendAuthCookies(Response, authResponse.AccessToken, authResponse.RefreshToken, loginDto.RememberMe);

            return Ok(authResponse);
        }
        [HttpPost("logout")]
        public async Task<ActionResult<AuthResponse>> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(userId == null)
            {
                return Unauthorized();
            }

            AuthResponse response = await _authenticationService.Logout(Guid.Parse(userId));

            _cookieService.DeleteAuthCookies(Response);

            return response;
        }
    }
}
