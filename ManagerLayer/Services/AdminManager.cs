﻿using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Services
{
    public class AdminManager : IAdminManager
    {
        //Dependency
        private readonly IAdminRepo adminRepo;

        public AdminManager(IAdminRepo adminRepo)
        {
            this.adminRepo = adminRepo;
        }

        //check duplicate email
        public async Task<bool> CheckEmailExistAsync(string email)
        {
            return await adminRepo.CheckEmailExistAsync(email);
        }

        //admin register
        public async Task<AdminEntity> RegisterAsync(RegisterModel model)
        {
            return await adminRepo.RegisterAsync(model);
        }

        //admin login
        public async Task<LoginResponseModel> LoginAsync(LoginModel loginModel)
        {
            return await adminRepo.LoginAsync(loginModel);
        }

        //forgot password
        public async Task<ForgotPasswordModel> ForgotPasswordAsync(string email)
        {
            return await adminRepo.ForgotPasswordAsync(email);
        }

        public async Task<bool> ResetPasswordAsync(string email, ResetPasswordModel reset)
        {
            return await adminRepo.ResetPasswordAsync(email, reset);
        }
    }
}
