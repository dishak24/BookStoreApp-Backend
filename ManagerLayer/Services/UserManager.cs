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

        public async Task<bool> CheckEmailExistAsync(string email)
        {
            return await userRepo.CheckEmailExistAsync(email);
        }


        public async Task<UserEntity> RegisterAsync(RegisterModel model)
        {
            return await userRepo.RegisterAsync(model);
        }

        // user login 
        public async Task<LoginResponseModel> LoginAsync(LoginModel loginModel)
        {
            return await userRepo.LoginAsync(loginModel);
        }
    }
}
