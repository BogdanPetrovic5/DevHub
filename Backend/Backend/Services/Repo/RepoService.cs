using Azure.Core;
using Backend.Dto;
using Backend.Dto.Repository;
using Backend.Interfaces.Repository;
using Backend.Models.Commit;
using Backend.Models.Repository;
using Backend.Responses;
using Backend.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography;

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
            var commitFiles = await _repoRepository.GetCommitFiles(repo.Id);
            var tree = BuildTree(files!, commitFiles, currentPath);
            var languages = BuildLanguages(files!);

            return repo.ToDetailsDto(tree, languages, latestCommit?.ToSummaryDto(), currentPath);
        }

        public async Task<List<RepoDto>> GetUserRepos(Guid userId)
        {
            return await _repoRepository.GetUserRepos(userId);
        }

        private List<TreeItemDto> BuildTree(List<RepoFile> files, List<RepoCommitFile> commitFiles, string currentPath = "")
        {
            var relevantFiles = string.IsNullOrEmpty(currentPath)
                ? files
                : files.Where(f => f.Path.StartsWith(currentPath + "/")).ToList();

            var relativePaths = relevantFiles
                .Select(rv => string.IsNullOrEmpty(currentPath) ? rv.Path : rv.Path[(currentPath.Length + 1)..])
                .ToList();

            return relativePaths.GroupBy(p => p.Split('/')[0])
                .Select(g =>
                {
                    var name = g.Key;
                    var fullPath = string.IsNullOrEmpty(currentPath) ? name : $"{currentPath}/{name}";
                    var isFolder = g.Any(p => p.Contains("/"));

                    var latestCf = isFolder
                        ? commitFiles.FirstOrDefault(cf => cf.Path.StartsWith(fullPath + "/"))
                        : commitFiles.FirstOrDefault(cf => cf.Path == fullPath);

                    return new TreeItemDto
                    {
                        Name = name,
                        Path = fullPath,
                        Type = isFolder ? "tree" : "blob",
                        LastCommitMessage = latestCf?.Commit?.Message ?? "",
                        LastCommitAt = latestCf?.Commit?.CreatedAt ?? default
                    };
                })
                .OrderBy(p => p.Type == "tree" ? 0 : 1)
                .ThenBy(p => p.Name)
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

        public async Task<RepoResponse> Upload(Guid repoId, Guid userId, RepoUploadRequest uploadRequest)
        {
           var repo = await _repoRepository.GetByIdAndOwner(repoId, userId);
            if (repo == null)
            {
                return new RepoResponse
                {
                    Success = false,
                    Message = "Repository not found."
                };
            }
            if (!uploadRequest.File.FileName.EndsWith(".zip"))
                return new RepoResponse { Success = false, Message = "Only .zip files are allowed" };

            if (repo.UserId != userId)
            {
                return new RepoResponse
                {
                    Success = false,
                    Message = "You do not have permission to upload to this repository."
                };
            }

            using var stream = uploadRequest.File.OpenReadStream();
            using var zip = new System.IO.Compression.ZipArchive(stream, System.IO.Compression.ZipArchiveMode.Read);

            var files = new List<RepoFile>();
            foreach (var entry in zip.Entries) { 
                if (string.IsNullOrEmpty(entry.Name)) continue;
                var segments = entry.FullName.Split('/');
                if (segments.Any(s => s == ".git" || s == "node_modules"))
                    continue;
                var ext = System.IO.Path.GetExtension(entry.Name).ToLower();
                if (_blockedExtensions.Contains(ext))
                    return new RepoResponse { Success = false, Message = $"File type {ext} is not allowed" };
                using var reader = new StreamReader(entry.Open());
                var content = await reader.ReadToEndAsync();
                var hash = ComputeHash(content);

                files.Add(new RepoFile
                {
                    Content = content,
                    Hash = hash,
                    Path = entry.FullName,
                    RepositoryId = repoId

                });

            

            }
            var commit = new RepoCommit
            {
                Message = uploadRequest.Message,
                RepositoryId = repoId,
                UserId = userId,
                CreatedAt = DateTime.Now
            };
            var commitFiles = files.Select(f => new RepoCommitFile
            {
                Path = f.Path,
                Content = f.Content,
                ChangeType = "Added",
            }).ToList();
            try
            {
                await _repoRepository.SaveUpload(files, commit, commitFiles);
                _cache.Remove($"repo-files-{repoId}");
                return new RepoResponse { Success = true, Message = "Upload successful" };
            }
            catch (DbUpdateException)
            {
                return new RepoResponse { Success = false, Message = "Failed to save upload." };
            }
        }

        private static readonly HashSet<string> _blockedExtensions = new()
        {
            ".exe", ".dll", ".bat", ".cmd", ".sh", ".ps1", ".vbs", ".msi", ".com", ".scr"
        };

        private string ComputeHash(string content)
        {
            var bytes = SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(content));
            return Convert.ToHexString(bytes).ToLower();
        }
    }
}
