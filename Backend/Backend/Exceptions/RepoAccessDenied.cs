namespace Backend.Exceptions
{
    public class RepoAccessDenied : Exception
    {
        public RepoAccessDenied() : base("Access to this repository is denied.") { }
    }
}
