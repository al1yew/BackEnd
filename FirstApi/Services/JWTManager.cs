using FirstApi.Data.Entities;
using FirstApi.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FirstApi.Services
{
    public class JWTManager : IJWTManager
    {
        private IConfiguration Configuration { get; }

        public JWTManager(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string GenerateToken(AppUser appUser, IList<string> roles)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, appUser.Id),
                new Claim(ClaimTypes.Name, appUser.UserName),
                new Claim(ClaimTypes.Email, appUser.Email),
                new Claim("EleBele","Test")
            };

            foreach (string role in roles)
            {
                Claim claim = new Claim(ClaimTypes.Role, role);
                claims.Add(claim);
            }

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration.GetSection("JWT:SecretKey").Value));
            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
            (
                issuer: Configuration.GetSection("JWT:Issuer").Value,
                audience: Configuration.GetSection("JWT:Audience").Value,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: signingCredentials
            );

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            string token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);

            return token;
        }

        public string GetUserNameByToken(string token)
        {
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            JwtSecurityToken DejwtSecurityToken = jwtSecurityTokenHandler.ReadToken(token) as JwtSecurityToken;

            List<Claim> deClaims = DejwtSecurityToken.Claims.ToList();

            return deClaims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
            //role de goturmek olur, id de goturmek olur
        }
    }
}
