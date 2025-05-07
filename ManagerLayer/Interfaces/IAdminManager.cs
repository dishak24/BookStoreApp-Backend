using RepositoryLayer.Entity;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interfaces
{
    public interface IAdminManager
    {
        //Checking email exist or not. Duplicate email not allowed
        public Task<bool> CheckEmailExistAsync(string email);

        //Register User
        public Task<AdminEntity> RegisterAsync(RegisterModel model);

        // user login 
        public Task<LoginResponseModel> LoginAsync(LoginModel loginModel);
    }
}
