namespace Backend.Dto.Search
{
    public class SearchResultDto
    {
        public List<RepoSearchItemDto> Repositories { get; set; } = [];
        public List<UserSearchItemDto> Users { get; set; } = [];
    }
}
