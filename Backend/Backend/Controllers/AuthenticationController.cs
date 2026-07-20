using Backend.Dto.Authentication;
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
        [HttpPost("cli-login")]
        public async Task<ActionResult> CliLogin([FromBody] LoginDto loginDto)
        {
            var userAgent = Request.Headers["User-Agent"].ToString();
            if (!userAgent.StartsWith("DevHubCLI"))
                return Forbid();

            var result = await _authenticationService.Login(loginDto);
            if (!result.Success) return Unauthorized(result);
            return Ok(new { accessToken = result.AccessToken, refreshToken = result.RefreshToken });
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
        [HttpDelete("logout")]
        public async Task<ActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if(refreshToken != null)
            {
               await _authenticationService.Logout(refreshToken);
            }

            

            _cookieService.DeleteAuthCookies(Response);

            return Ok();
        }
    }
}
