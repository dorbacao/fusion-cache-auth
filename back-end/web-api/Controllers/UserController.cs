using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using web_api.Model;

namespace web_api.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Reflection;
    using web_api.Services;

    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        const string LOGIN = "vinicius";
        const string PWD = "123";

        public IMemoryCache Cache { get; }

        public UserController(IMemoryCache cache)
        {
            Cache = cache;
        }

        private bool ValidateLoginAndPassword(LoginModel model)
        {
            return model?.Login == LOGIN && model?.Password == PWD;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult LoginAsync([FromBody] LoginModel model)
        {
            if (ValidateLoginAndPassword(model))
            {
                var service = new TokenService("tC7gH2S8fV7M9W5xK6zR8J4dH7tF8Q5b", "yourIssuer", "yourAudience");
                var token = service.GenerateToken(model.Login);

                var cacheToken = Cache.GetOrCreate(token.SessionId, entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return token;
                });

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));

                Cache.Set(token.SessionId, token, cacheEntryOptions);
                var valie = Cache.Get(token.SessionId);

                Response.Cookies.Append("session-id", token.SessionId,
                    new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(30)
                    });

                return Ok(token);
            }

            return Unauthorized();
        }

        [HttpGet("name")]
        public IActionResult GetNameAsync()
        {
            return Ok(User.Claims.FirstOrDefault().Value);
        }
    }


}
