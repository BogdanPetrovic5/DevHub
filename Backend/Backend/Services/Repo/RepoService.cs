using Backend.Dto;
using Backend.Dto.Repository;
using Backend.Interfaces.Repository;
using Backend.Models.Commit;
using Backend.Models.Repository;
using Backend.Responses;
using Backend.Utility;
using Microsoft.Extensions.Caching.Memory;

namespace Backend.Services.Repository
{
    public class RepoService : IRepoService
    {
        private readonly IRepoRepository _repoRepository;
        private readonly IMemoryCache _cache;

        public RepoService(IRepoRepository repoRepository, IMemoryCache cache)
        {
            _repoRepository = repoRepository;
            _cache = cache;
        }

        public async Task<RepoResponse> Create(CreateRepoDto createRepoDto, Guid userId)
        {
            if (createRepoDto == null)
            {
                return new RepoResponse
                {
                    Success = false,
                    Message = "CreateRepoDto cannot be null."
                };
            }

            Repo newRepository = new Repo
            {
                Name = createRepoDto.Name.Trim().Replace(" ", "-").ToLower(),
                Description = createRepoDto.Description,
                IsPrivate = createRepoDto.IsPrivate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = userId
            };

            return await _repoRepository.Create(newRepository);
        }

        public async Task<RepoDetailsDto?> GetRepo(string username, string repoName, Guid? userId, string currentPath)
        {
            var repo = await _cache.GetOrCreateAsync($"repo-{username}-{repoName}", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _repoRepository.GetByUsernameAndName(username, repoName);
            });
            if (repo == null) return null;

            if (repo.IsPrivate && repo.UserId != userId) return null;

            var files = await _cache.GetOrCreateAsync($"repo-files-{repo.Id}", async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10);
                return await _repoRepository.GetFiles(repo.Id);
            });

            var latestCommit = await _repoRepository.GetLatestCommit(repo.Id);
            var tree = BuildTree(files!, currentPath);
            var languages = BuildLanguages(files!);

            return repo.ToDetailsDto(tree, languages, latestCommit?.ToSummaryDto(), currentPath);
        }

        public async Task<List<RepoDto>> GetUserRepos(Guid userId)
        {
            return await _repoRepository.GetUserRepos(userId);
        }

        private List<TreeItemDto> BuildTree(List<RepoFile> files, string currentPath = "")
        {
            var relevatFiles = string.IsNullOrEmpty(currentPath) 
                ? files 
                : files.Where(f => f.Path.StartsWith(currentPath + "/")).ToList();

            var relativePaths = relevatFiles.Select(rv => string.IsNullOrEmpty(currentPath) ? rv.Path : rv.Path[(currentPath.Length + 1)..]).ToList();

            return relativePaths.GroupBy(p => p.Split('/')[0])
                .Select(g => new TreeItemDto
                {
                    Name = g.Key,
                    Path = string.IsNullOrEmpty(currentPath) ? g.Key : $"{currentPath}/{g.Key}",
                    Type = g.Any(p => p.Contains("/")) ? "tree" : "blob"
                })
                .ToList();

        }


        private static readonly Dictionary<string, string> _extensionToLanguage = new()
        {
            { ".ts", "TypeScript" },
            { ".cs", "C#" },
            { ".cpp", "C++" },
            {".py", "Python" },
            {".c", "C" },
            { ".scss", "SCSS" },
            { ".html", "HTML" },
            { ".json", "JSON" },
            { ".js", "JavaScript" },
            { ".css", "CSS" },
            { ".md", "Markdown" },
        };

        private List<RepoLanguageDto> BuildLanguages(List<RepoFile> files)
        {
            var counts = files
                .Select(f => System.IO.Path.GetExtension(f.Path).ToLower())
                .Where(ext => _extensionToLanguage.ContainsKey(ext))
                .GroupBy(ext => _extensionToLanguage[ext])
                .Select(g => new { Language = g.Key, Count = g.Count() })
                .ToList();

            var total = counts.Sum(c => c.Count);
            if (total == 0) return [];

            return counts
                .OrderByDescending(c => c.Count)
                .Select(c => new RepoLanguageDto
                {
                    Language = c.Language,
                    FileCount = c.Count,
                    Percentage = Math.Round((double)c.Count / total * 100, 1)
                })
                .ToList();
        }
    }
}
