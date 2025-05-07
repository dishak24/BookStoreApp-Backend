using RepositoryLayer.Entity;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IAdminRepo
    {
        //Checking email exist or not. Duplicate email not allowed
        public Task<bool> CheckEmailExistAsync(string email);

        //Register admin
        public Task<AdminEntity> RegisterAsync(RegisterModel model);

        // admins login 
        public Task<LoginResponseModel> LoginAsync(LoginModel loginModel);

        //admins Forgot password method.
        public Task<ForgotPasswordModel> ForgotPasswordAsync(string email);

        // admins Reset password
        public Task<bool> ResetPasswordAsync(string email, ResetPasswordModel reset);

    }
}
