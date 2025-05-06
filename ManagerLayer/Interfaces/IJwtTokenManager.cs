using CommonLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interfaces
{
    public interface IJwtTokenManager
    {
        Task<string> GenerateToken(JwtModel jwtModel);
        Task<string> GenerateRefreshToken();
        public Task SaveRefreshTokenInDb(int userId, string refreshToken);
    }
}
