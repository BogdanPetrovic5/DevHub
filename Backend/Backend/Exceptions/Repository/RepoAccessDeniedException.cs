namespace Backend.Exceptions.Repository
{
    public class RepoAccessDeniedException : AppException
    {
        public override int StatusCode => StatusCodes.Status403Forbidden;
        public RepoAccessDeniedException() : base("Access to this repository is denied.") { }
    }
}
