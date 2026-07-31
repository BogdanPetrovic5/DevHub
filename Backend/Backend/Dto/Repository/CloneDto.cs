using Backend.Models.Repository;

namespace Backend.Dto.Repository
{
    public class CloneDto
    {
        public Guid RepoId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string OwnerUsername { get; set; } = string.Empty;
        public IEnumerable<RepoFile> Files { get; set; } = Enumerable.Empty<RepoFile>();
    }
}
