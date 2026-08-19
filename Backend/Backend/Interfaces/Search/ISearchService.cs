using Backend.Dto.Search;

namespace Backend.Interfaces.Search
{
    public interface ISearchService
    {
        public Task<SearchResultDto> SearchAsync(string query, Guid userId);
    }
}
