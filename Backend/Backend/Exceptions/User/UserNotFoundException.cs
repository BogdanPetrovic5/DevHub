namespace Backend.Exceptions.User
{
    public class UserNotFoundException : AppException
    {
        public override int StatusCode => StatusCodes.Status404NotFound;
        public UserNotFoundException(string username) : base($"User '{username}' not found.") { }
    }
}
