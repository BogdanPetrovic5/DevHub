using Backend.Dto;
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
    }
}
