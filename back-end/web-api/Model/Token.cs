using System.Security.Claims;

namespace web_api.Model
{
    public class Token
    {
        public string AccessToken { get; set; }
        public string SessionId { get; set; }
        public IEnumerable<Claim> Claims { get; internal set; }
    }

    public class LoginModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
