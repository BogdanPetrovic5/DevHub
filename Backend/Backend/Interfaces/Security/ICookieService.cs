namespace Backend.Interfaces.Security
{
    public interface ICookieService
    {
        void AppendAuthCookies(HttpResponse response, string accessToken, string refreshToken);
    }
}
