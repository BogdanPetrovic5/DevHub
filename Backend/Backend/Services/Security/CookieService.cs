using Backend.Interfaces.Security;

namespace Backend.Services.Security
{
    public class CookieService : ICookieService
    {
        public void AppendAuthCookies(HttpResponse response, string accessToken, string refreshToken, bool rememberMe)
        {
           

            response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = DateTime.UtcNow.AddMinutes(15)
            });
            response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
                Expires = rememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddDays(7)
            });
        }

        public void DeleteAuthCookies(HttpResponse response)
        {
            response.Cookies.Delete("accessToken");
            response.Cookies.Delete("refreshToken");
        }
    }
}
