using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net.Sockets;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using web_api.Model;

public class SessionIdAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IMemoryCache _cache;

    public SessionIdAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IMemoryCache cache)
        : base(options, logger, encoder, clock)
    {
        _cache = cache;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("session-id", out var sessionId))
        {
            return AuthenticateResult.Fail("Session-ID header not found.");
        }

        string key = sessionId.ToString();

        if (_cache.TryGetValue<Token>(key, out var session)) {
            var username = session;

            if (username == null)
            {
                return AuthenticateResult.Fail("Invalid session-id.");
            }

            var identity = new ClaimsIdentity(session.Claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        return AuthenticateResult.NoResult();


    }
}
