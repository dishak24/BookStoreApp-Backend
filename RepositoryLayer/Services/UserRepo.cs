using CommonLayer.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class UserRepo : IUserRepo
    {
        private readonly BookDBContext context;
        
        public UserRepo(BookDBContext context)
        {
            this.context = context;
        }

        //Checking email exist or not. Duplicate email not allowed
        public async Task<bool> CheckEmailExistAsync(string email)
        {
            var result = await this.context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (result == null)
            {
                return false;
            }
            return true;
        }

        //Register User
        public async Task<UserEntity> RegisterAsync(RegisterModel model)
        {
            UserEntity user = new UserEntity();

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Mobile = model.Mobile;
            user.Role = "User";
            user.Password = EncodePasswordToBase64(model.Password);

            await context.Users.AddAsync(user);
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




    }
}
