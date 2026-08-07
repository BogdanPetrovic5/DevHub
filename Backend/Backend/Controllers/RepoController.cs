using Backend.Dto.Repository;
using Backend.Exceptions;
using Backend.Interfaces.Repository;
using Backend.Models.Repository;
using Backend.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.IO.Compression;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/repo")]
    [ApiController]
    public class RepoController : ControllerBase
    {
        private readonly IRepoService _repoService;
        private readonly ILogger<RepoController> _logger;
        public RepoController(IRepoService repoService, ILogger<RepoController> logger)
        {
            _repoService = repoService;
            _logger = logger;
        }
        [Authorize]
        [HttpPost("new")]
        public async Task<ActionResult<RepoResponse>> CreateRepo([FromBody] CreateRepoDto createRepoDto)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            RepoResponse repoResponse = await _repoService.Create(createRepoDto, userId);

            if (repoResponse.Success)
            {
                return Ok(repoResponse);
            }
            else
            {
                return BadRequest(repoResponse);
            }
        }
        [EnableRateLimiting("clone")]
        [Authorize]
        [HttpGet("{username}/{repoName}/clone")]
        public async Task<ActionResult> CloneRepo(string username, string repoName)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            Guid? userId = userIdClaim != null ? Guid.Parse(userIdClaim.Value) : null;
            try
            {
                CloneDto? cloneResponse = await _repoService.GetAllFiles(username, repoName, userId);
                if (cloneResponse?.Files == null) return NotFound();
                using var memoryStream = new MemoryStream();
                using (var zip = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in cloneResponse.Files)
                    {
                        var entry = zip.CreateEntry(file.Path, CompressionLevel.Fastest);
                        using var streamWriter = new StreamWriter(entry.Open());
                        await streamWriter.WriteAsync(file.Content);
                    }
                }
                memoryStream.Position = 0;
                Response.Headers.Append("X-Repo-Id", cloneResponse.RepoId.ToString());
            
                return File(memoryStream.ToArray(), "application/zip", "repo.zip");
            }
            catch (RepoAccessDenied ex) {
                return StatusCode(403, new { message = ex.Message });
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

        [Authorize]
        [HttpPost("{repoId}/upload")]
        public async Task<ActionResult<RepoResponse>> Upload(Guid repoId, [FromForm] RepoUploadRequest uploadRequest)
        {
            _logger.LogInformation("Upload called");

            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            RepoResponse result = await _repoService.Upload(repoId, userId, uploadRequest);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }

        }

        [AllowAnonymous]
        [HttpGet("{username}/{repoName}")]
        public async Task<ActionResult<RepoDetailsDto>> GetRepo(string username, string repoName, [FromQuery] string path = "")
        {
            _logger.LogInformation("GetDetails called");
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            Guid? userId = userIdClaim != null ? Guid.Parse(userIdClaim.Value) : null;

            var repoDto = await _repoService.GetRepo(username, repoName, userId, path);

            if (repoDto == null) return NotFound();
            return Ok(repoDto);
        }
        [AllowAnonymous]
        [HttpGet("{username}/{repoName}/blob")]
        public async Task<ActionResult<RepoFileContentDto>> ViewFile(string username, string reponame, [FromQuery] string path)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            Guid? userId = userIdClaim != null ? Guid.Parse(userIdClaim.Value) : null;
            RepoDetailsDto? repo = await _repoService.GetRepo(username, reponame, userId, path);

            if (repo == null) return NotFound();

            if (repo.IsPrivate && userId != repo.OwnerId) return Forbid();

            RepoFileContentDto? fileContent = await _repoService.GetFileContent(repo.Id, path);

            if (fileContent == null) return NotFound();
            return Ok(fileContent);

        }
        [AllowAnonymous]
        [HttpGet("{username}/{repoName}/commits")]
        public async Task<ActionResult<List<RepoCommitSummaryDto>>> GetCommits(string username, string repoName)
        {
            var userIdClaims = User.FindFirst(ClaimTypes.NameIdentifier);
            Guid? userId = userIdClaims != null ? Guid.Parse(userIdClaims.Value) : null;

            try
            {
                List<RepoCommitSummaryDto>? repoCommitSummaryDtos = await _repoService.GetRepoCommits(userId, username, repoName);
                return Ok(repoCommitSummaryDtos);
            }
            catch (RepoAccessDenied)
            {
                return Forbid();
            }
        }
        [EnableRateLimiting("push")]
        [Authorize]
        [HttpPut("{repoId}/push")]
        public async Task<ActionResult<RepoResponse>> Push(Guid repoId, [FromBody] PushRequestDto pushRequest)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _repoService.Push(repoId, userId, pushRequest);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
        [AllowAnonymous]
        [HttpGet("{username}/{repoName}/commits/{commitId}")]
        public async Task<ActionResult<CommitFilesDto?>> GetCommitDetails(string username, string repoName, Guid commitId)
        {
            var userIdClaims = User.FindFirst(ClaimTypes.NameIdentifier);
            Guid? userId = userIdClaims != null ? Guid.Parse(userIdClaims.Value) : null;

            try
            {
                CommitFilesDto? commitFiles = await _repoService.GetCommitFiles(username, repoName, commitId, userId);
                return commitFiles;
            }
            catch (RepoAccessDenied)
            {
                return Forbid();
            }
        }

        [Authorize]
        [HttpGet("activity")]
        public async Task<ActionResult<List<ActivityDto>>> GetUserActivity()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            List<ActivityDto> activities = await _repoService.GetActivity(userId);
            return Ok(activities);
        }
        [EnableRateLimiting("pull")]
        [Authorize]
        [HttpPost("{repoId}/pull")]
        public async Task<ActionResult> Pull(Guid repoId, [FromBody] PullRequestDto pullRequest)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            try
            {
                PullResponseDto result = await _repoService.Pull(repoId, userId, pullRequest);
                return Ok(result);
            }
            catch (RepoNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (RepoAccessDenied ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
        }
    }
}
