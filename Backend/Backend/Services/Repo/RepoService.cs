
using Backend.Dto;
using Backend.Interfaces.Repository;
using Backend.Models.Repository;
using Backend.Responses;

namespace Backend.Services.Repository
{
    public class RepoService : IRepoService
    {
        private readonly IRepoRepository _repoRepository;
        public RepoService(IRepoRepository repoRepository)
        {
            _repoRepository = repoRepository;
        }
        public async Task<RepoResponse> Create(CreateRepoDto createRepoDto, Guid userId)
        {
            if(createRepoDto == null)
            {
                return new RepoResponse
                {
                    Success = false,
                    Message = "CreateRepoDto cannot be null."
                };
            }
            Repo newRepository = new Repo
            {
                Name = createRepoDto.Name,
                Description = createRepoDto.Description,
                IsPrivate = createRepoDto.IsPrivate,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                UserId = userId
            };

            RepoResponse response = await _repoRepository.Create(newRepository);

            return response;
        }

        public async Task<List<RepoDto>> GetUserRepos(Guid userId)
        {
            return await _repoRepository.GetUserRepos(userId);
        }
    }
}
