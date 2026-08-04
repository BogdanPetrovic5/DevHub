namespace Backend.Exceptions
{
    public class RepoNotFoundException : Exception
    {
        public RepoNotFoundException() : base("Repository not found.") { }
    }
}
