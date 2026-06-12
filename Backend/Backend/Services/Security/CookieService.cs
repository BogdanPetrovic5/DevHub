using Backend.Interfaces.Security;

namespace Backend.Services.Security
{
    public class CookieService : ICookieService
    {
        public void AppendAuthCookies(HttpResponse response, string accessToken, string refreshToken)
        {
           

            response.Cookies.Append("accessToken", accessToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Secure = false, //for dev phase
                Expires = DateTime.UtcNow.AddMinutes(15)
            });
            response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Secure = false, //for dev phase
                Expires = DateTime.UtcNow.AddDays(7)
            });
        }
    }
}
