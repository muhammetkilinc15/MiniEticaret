using ApiGateway.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

namespace ApiGateway.Services
{
    public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
    {
        private IOptions<JwtOptions> _options = options;

        public string CreateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };


            SigningCredentials credentials = new(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.SecretKey)),
                SecurityAlgorithms.HmacSha256
            );

            JwtSecurityToken token = new(
                issuer: _options.Value.Issuer,
                audience: _options.Value.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );
            JwtSecurityTokenHandler handler = new();
            return handler.WriteToken(token);
        }
    }
    public class JwtOptions
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string SecretKey { get; set; } = default!; 
    }

}
