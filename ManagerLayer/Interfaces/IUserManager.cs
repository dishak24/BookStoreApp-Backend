
using RepositoryLayer.Entity;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interfaces
{
    public interface IUserManager
    {
        //Checking email exist or not. Duplicate email not allowed
        public Task<bool> CheckEmailExistAsync(string email);

        //Register User
        public Task<UserEntity> RegisterAsync(RegisterModel model);

        // user login 
        public Task<LoginResponseModel> LoginAsync(LoginModel loginModel);
    }
}
