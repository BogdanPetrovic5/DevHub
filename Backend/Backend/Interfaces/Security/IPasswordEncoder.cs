namespace Backend.Interfaces.Security
{
    public interface IPasswordEncoder
    {
        string EncodePassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
