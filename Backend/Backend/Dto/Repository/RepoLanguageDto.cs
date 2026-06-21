namespace Backend.Dto.Repository
{
    public class RepoLanguageDto
    {
        public string Language { get; set; } = String.Empty;
        public int FileCount { get; set; }
        public double Percentage { get; set; }
    }
}
