
using ManagerLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System;
using RepositoryLayer.Context;
using ManagerLayer.Interfaces;
using System.Linq;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Helpers;
using RepositoryLayer.Models;

namespace BookStoreApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BookDBContext context;
        private readonly IConfiguration configuration;
        private readonly JwtTokenManager jwtToken;
        public AuthController(BookDBContext context, IConfiguration configuration, JwtTokenManager jwtToken)
        {
            this.context = context;
            this.configuration = configuration;
            this.jwtToken = jwtToken;
        }

        // to refresh the user token
        [HttpPost("users-refresh-Token")]
        public async Task<IActionResult> UserRefreshTokenAsync(RefreshTokenModel model)
        {
            var user = context.Users.FirstOrDefault(u => u.RefreshToken == model.RefreshToken && 
                                                        u.RefreshTokenExpiryTime > DateTime.Now);

            if (user == null)
            {
                return Unauthorized(new 
                { 
                    success = false, 
                    message = "Invalid or expired refresh token" 
                });
            }

            var newAccessToken = await jwtToken.GenerateToken(new JwtModel
            {
                Id = user.UserId,
                Email = user.Email,
                Role = user.Role
            });

            var newRefreshToken = await jwtToken.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            int refreshExpiryDays = int.Parse(configuration["Jwt:RefreshTokenExpiration"]);
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshExpiryDays);

     
            context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Token refreshed",
                data = new
                {
                    accessToken = newAccessToken,
                    refreshToken = newRefreshToken
                }
            });
        }

        // to refresh the admin token
        [HttpPost("admins-refresh-token")]
        
        public async Task<IActionResult> AdminRefreshTokenAsync(RefreshTokenModel model)
        {
            var admin = context.Admins.FirstOrDefault(u => u.RefreshToken == model.RefreshToken &&
                                                        u.RefreshTokenExpiryTime > DateTime.Now);

            if (admin == null)
            {
                return Unauthorized(new 
                { 
                    success = false, 
                    message = "Invalid or expired refresh token" 
                });
            }

            var newAccessToken = await jwtToken.GenerateToken(new JwtModel
            {
                Id = admin.AdminId,
                Email = admin.Email,
                Role = admin.Role
            });

            var newRefreshToken = await jwtToken.GenerateRefreshToken();

            admin.RefreshToken = newRefreshToken;
            int refreshExpiryDays = int.Parse(configuration["Jwt:RefreshTokenExpiration"]);
            admin.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshExpiryDays);


            context.SaveChanges();

            return Ok(new
            {
                success = true,
                message = "Token refreshed",
                data = new
                {
                    accessToken = newAccessToken,
                    refreshToken = newRefreshToken
                }
            });
        }

    }
}
