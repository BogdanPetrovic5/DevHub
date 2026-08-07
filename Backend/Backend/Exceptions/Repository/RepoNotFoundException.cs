namespace Backend.Exceptions.Repository
{
    public class RepoNotFoundException : AppException
    {
        public override int StatusCode => StatusCodes.Status404NotFound;
        public RepoNotFoundException() : base("Repository not found.") { }
    }
}
