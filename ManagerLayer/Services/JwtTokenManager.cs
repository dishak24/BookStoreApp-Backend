using CommonLayer.Model;
using ManagerLayer.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace ManagerLayer.Services
{
    public class JwtTokenManager : IJwtTokenManager
    {
        private readonly IConfiguration configuration;

        public JwtTokenManager(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<string> GenerateToken(JwtModel jwtModel)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim("EmailId", jwtModel.Email),
            new Claim("UserId", jwtModel.Id.ToString()),
            new Claim(ClaimTypes.Role, jwtModel.Role)
        };

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

