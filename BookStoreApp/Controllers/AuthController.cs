
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
    //[Route("api/[controller]")]
    //[ApiController]
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

        [HttpPost()]
        [Route("refreshToken")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenModel model)
        {
            var user = context.Users.FirstOrDefault(u => u.RefreshToken == model.RefreshToken && 
                                                        u.RefreshTokenExpiryTime > DateTime.Now);

            if (user == null)
            {
                return Unauthorized(new { success = false, message = "Invalid or expired refresh token" });
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

    }
}
