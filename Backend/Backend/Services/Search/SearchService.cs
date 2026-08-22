using Backend.Dto.Search;
using Backend.Interfaces.Search;
using Backend.Utility;

namespace Backend.Services.Search
{
    public class SearchService : ISearchService
    {
        private readonly ISearchRepository _searchRepository;
        public SearchService(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }
        public async Task<SearchResultDto> SearchAsync(string query, Guid userId)
        {
            var repos = await _searchRepository.SearchRepos(query, userId);
            var users = await _searchRepository.SearchUsers(query);

            return new SearchResultDto
            {
                Repositories = repos.Select(r => r.ToSearchDto()).ToList(),
                Users = users.Select(u => u.ToSearchDto()).ToList()
            };
        }
    }
}
