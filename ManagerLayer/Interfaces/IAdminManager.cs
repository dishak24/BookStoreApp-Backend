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

        //Register admin
        public Task<AdminEntity> RegisterAsync(RegisterModel model);

        // admin login 
        public Task<LoginResponseModel> LoginAsync(LoginModel loginModel);

        // admin Forgot password method.
        public Task<ForgotPasswordModel> ForgotPasswordAsync(string email);

        // admins Reset password
        public Task<bool> ResetPasswordAsync(string email, ResetPasswordModel reset);
    }
}
