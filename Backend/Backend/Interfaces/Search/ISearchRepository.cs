using Backend.Models;
using Backend.Models.Repository;

namespace Backend.Interfaces.Search
{
    public interface ISearchRepository
    {
        public Task<List<Repo>> SearchRepos(string query, Guid userId);
        public Task<List<Backend.Models.User>> SearchUsers(string query);
    }
}
