
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace RepositoryLayer.Helpers
{
    public class JwtTokenManager
    {
        private readonly IConfiguration configuration;
        private readonly BookDBContext context;

        public JwtTokenManager(IConfiguration configuration, BookDBContext context)
        {
            this.configuration = configuration;
            this.context = context;
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
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );

            //convert token to string using writeToken()
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // to generate refresh token
        public async Task<string> GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using (var randomNumber = RandomNumberGenerator.Create())
            {
                randomNumber.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        // to save the refresh token in the db
        public async Task SaveRefreshTokenInDb(int userId, string refreshToken)
        {
            // get user by user id
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId); 

            if (user != null)
            {
                // Assign the refresh token and set expiry time
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(Convert.ToDouble(configuration["Jwt:RefreshTokenExpiration"])); // expiry from config

                // Save changes directly
                await context.SaveChangesAsync();
            }
        }

    }
}

