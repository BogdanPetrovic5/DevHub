using Backend.Interfaces.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }
        [HttpGet("{q}")]
        public async Task<IActionResult> Search([FromQuery]string q)
        {
            var userIdClaims = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            Guid userId = userIdClaims != null ? Guid.Parse(userIdClaims.Value) : Guid.Empty;

            var searchResult = await _searchService.SearchAsync(q, userId);
            
            return Ok(searchResult);
        }
    }
}
