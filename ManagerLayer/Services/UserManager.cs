using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;

namespace ManagerLayer.Services
{
    public class UserManager : IUserManager
    {
        //Dependency
        private readonly IUserRepo userRepo;

        public UserManager(IUserRepo userRepo)
        {
            this.userRepo = userRepo;
        }

        //check email already exist or not
        public async Task<bool> CheckEmailExistAsync(string email)
        {
            return await userRepo.CheckEmailExistAsync(email);
        }


        //register user
        public async Task<UserEntity> RegisterAsync(RegisterModel model)
        {
            return await userRepo.RegisterAsync(model);
        }

        // user login 
        public async Task<LoginResponseModel> LoginAsync(LoginModel loginModel)
        {
            return await userRepo.LoginAsync(loginModel);
        }

        //users forgot password
        public async Task<ForgotPasswordModel> ForgotPassword(string email)
        {
            return await userRepo.ForgotPassword(email);
        }

        //users reset password
        public async Task<bool> ResetPassword(string email, ResetPasswordModel reset)
        {
            return await userRepo.ResetPassword(email, reset);
        }
    }
}
