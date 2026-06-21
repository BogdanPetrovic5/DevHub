namespace Backend.Dto.Repository
{
    public class RepoUploadRequest
    {
      
        public IFormFile File { get; set; }
        public string Message { get; set; }
    }
}
