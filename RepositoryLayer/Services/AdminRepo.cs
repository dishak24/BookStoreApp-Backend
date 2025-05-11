using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Helpers;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class AdminRepo : IAdminRepo
    {
        private readonly BookDBContext context;
        private readonly JwtTokenManager tokenManager;

        public AdminRepo(BookDBContext context, JwtTokenManager tokenManager)
        {
            this.context = context;
            this.tokenManager = tokenManager;
        }

        //Checking email exist or not. Duplicate email not allowed
        public async Task<bool> CheckEmailExistAsync(string email)
        {
            var result = await this.context.Admins.FirstOrDefaultAsync(x => x.Email == email);
            if (result == null)
            {
                return false;
            }
            return true;
        }

        //Register Admin
        public async Task<AdminEntity> RegisterAsync(RegisterModel model)
        {
            AdminEntity user = new AdminEntity();

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Mobile = model.Mobile;
            user.Role = "Admin";
            user.Password = EncodePasswordToBase64(model.Password);

            await context.Admins.AddAsync(user);
            context.SaveChanges();
            return user;
        }

        //To encrypt Password For security
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encryption_Data = new byte[password.Length];
                encryption_Data = System.Text.Encoding.UTF8.GetBytes(password);
                string encoded_Data = Convert.ToBase64String(encryption_Data);
                return encoded_Data;
            }
            catch (Exception e)
            {
                throw new Exception("Error in base64 encode " + e.Message);
            }
        }

        // user login 
        public async Task<LoginResponseModel> LoginAsync(LoginModel loginModel)
        {
            var encodedPassword = EncodePasswordToBase64(loginModel.Password);
            var user = await context.Admins
                .FirstOrDefaultAsync(u => u.Email == loginModel.Email && u.Password == encodedPassword);

            if (user == null) 
                return null;

            var accessToken = await tokenManager.GenerateToken(new JwtModel
            {
                Id = user.AdminId,
                Email = user.Email,
                Role = user.Role
            });

            var refreshToken = await tokenManager.GenerateRefreshToken();
            await tokenManager.SaveAdminRefreshTokenInDb(user.AdminId, refreshToken);

            return new LoginResponseModel
            {
                Name = user.FullName,
                Email = user.Email,
                AccessToken = accessToken,
                RefreshToken = refreshToken
               
            };

        }

        //Forgot password method.
        public async Task<ForgotPasswordModel> ForgotPasswordAsync(string email)
        {
            AdminEntity user = context.Admins.ToList().Find(user => user.Email == email);

            ForgotPasswordModel forgotPassword = new ForgotPasswordModel();
            forgotPassword.Email = user.Email;
            forgotPassword.UserId = user.AdminId;

            forgotPassword.Token = await tokenManager.GenerateToken(new JwtModel
            {
                Email = user.Email,
                Id = user.AdminId,
                Role = user.Role
               

            });

            return forgotPassword;
        }

        //To Reset the password:
        //1. check email exist or not
        //2. then change password
        public async Task<bool> ResetPasswordAsync(string email, ResetPasswordModel reset)
        {
            var user = context.Admins.ToList().Find(u => u.Email == email);

            if (await CheckEmailExistAsync(user.Email))
            {
                user.Password = EncodePasswordToBase64(reset.ConfirmPassword);
                context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
