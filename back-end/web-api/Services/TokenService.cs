using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using web_api.Model;


namespace web_api.Services
{
    public class TokenService
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(string key, string issuer, string audience)
        {
            _key = key;
            _issuer = issuer;
            _audience = audience;
        }

        public Token GenerateToken(string username)
        {
            var claims = GenerateClaims(username);

            var sessionId = claims.First(a => a.Type == JwtRegisteredClaimNames.Jti).Value;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            var secToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new Token() { AccessToken = secToken, SessionId = sessionId, Claims = claims };
        }

        public IEnumerable<Claim> GenerateClaims(string username)
        {
            yield return new Claim(JwtRegisteredClaimNames.Sub, username);
            yield return new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());

            for (int i = 0; i < 300; i++)
            {
                yield return new Claim(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            }
        }
    }
}
