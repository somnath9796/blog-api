using CodePulse.API.Repository.Interface;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CodePulse.API.Repository.Implementation
{
    public class TokenRespoitory : ITokenRepository
    {
        private readonly IConfiguration _configuration;
        public TokenRespoitory(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            //Create Claims
            var claims = new List<Claim>
            { 
            new Claim(ClaimTypes.Email, user.Email),
            };

            claims.AddRange(roles.Select(roles => new Claim(ClaimTypes.Role, roles)));

            //JWT Security Parameters
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var credintials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: _configuration["Jwt:Issuer"],
                audience : _configuration["Jwt:Audience"],
                claims : claims,
                expires :DateTime.Now.AddMinutes(15),
                signingCredentials : credintials
            );

            //Return Token
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
