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
            var repos = _searchRepository.SearchRepos(query, userId);
            var users = _searchRepository.SearchUsers(query);

            await Task.WhenAll(repos, users);
            return new SearchResultDto
            {
                Repositories = repos.Result.Select(r => r.ToSearchDto()).ToList(),
                Users = users.Result.Select(u => u.ToSearchDto()).ToList()
            };
        }
    }
}
