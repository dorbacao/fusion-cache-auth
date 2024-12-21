using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<SessionIdAuthenticationHandler>();
builder.Services.AddMemoryCache();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "MultiScheme";
    //options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("Bearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "yourIssuer",
        ValidAudience = "yourAudience",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("tC7gH2S8fV7M9W5xK6zR8J4dH7tF8Q5b"))
    };
}).AddScheme<AuthenticationSchemeOptions, SessionIdAuthenticationHandler>("SessionId", options => { })
.AddPolicyScheme("MultiScheme", "Authorization Bearer or SessionId", options =>
{
    options.ForwardDefaultSelector = context =>
    {
        if (context.Request.Headers.ContainsKey("session-id"))
        {
            return "SessionId";
        }
        return JwtBearerDefaults.AuthenticationScheme;
    };
});
;

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthentication(); app.UseAuthorization();

app.UseCors("AllowAllOrigins");

app.MapControllers();

app.Run();
