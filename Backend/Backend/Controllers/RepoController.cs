using Backend.Dto;
using Backend.Dto.Repository;
using Backend.Interfaces.Repository;
using Backend.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/repo")]
    [ApiController]
    public class RepoController : ControllerBase
    {
        private readonly IRepoService _repoService;
        public RepoController(IRepoService repoService)
        {
            _repoService = repoService;
        }
        [Authorize]
        [HttpPost("new")]
        public async Task<ActionResult<RepoResponse>> CreateRepo([FromBody] CreateRepoDto createRepoDto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            RepoResponse repoResponse = await _repoService.Create(createRepoDto, userId);

            if(repoResponse.Success)
            {
                return Ok(repoResponse);
            }
            else
            {
                return BadRequest(repoResponse);
            }
        }
        [Authorize]
        [HttpGet("user")]
        public async Task<ActionResult<List<RepoDto>>> GetUserRepos()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            List<RepoDto> repoDtos = await _repoService.GetUserRepos(userId);
            return Ok(repoDtos);
        }

        //[Authorize]
        //[HttpPost("{name}/upload")]
        //public async Task<IActionResult> Upload(string name, [FromForm] IFormFile formFile)
        //{
        //    var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        //}


        [HttpGet("{username}/{repoName}")]
        public async Task<ActionResult<RepoDetailsDto>> GetRepo(string username, string repoName, [FromQuery] string path = "")
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            Guid? userId = userIdClaim != null ? Guid.Parse(userIdClaim.Value) : null;

            var repoDto = await _repoService.GetRepo(username, repoName, userId, path);

            if (repoDto == null) return NotFound();
            return Ok(repoDto);
        }
    }
}
