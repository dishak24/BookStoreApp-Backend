using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Helpers;
using RepositoryLayer.Models;
using System;
using System.Threading.Tasks;

namespace BookStoreApp.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminManager manager;

        public AdminController(IAdminManager manager)
        {
            this.manager = manager;
        }

        //admin registration
        [HttpPost]
        [Route("adminRegister")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (model == null)
            {
                return BadRequest("data is null");
            }
            // to checking email already used or not
            var check = await manager.CheckEmailExistAsync(model.Email);

            if (check)
            {
                return BadRequest(new ResponseModel<UserEntity>
                {
                    Success = false,
                    Message = "Email already exist! Please, Enter another EmailId."
                });
            }
            else
            {
                var result = await manager.RegisterAsync(model);

                if (result != null)
                {
                    return Ok(new ResponseModel<AdminEntity>
                    {
                        Success = true,
                        Message = "Registration Successfull !",
                        Data = result
                    });
                }
                return BadRequest(new ResponseModel<AdminEntity>
                {
                    Success = false,
                    Message = "Registration failed !!!!",
                    Data = result
                });
            }
        }


        //admin login
        [HttpPost()]
        [Route("adminLogin")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var result = await manager.LoginAsync(model);

            if (result == null)
            {
                return Unauthorized(new { Success = false, Message = "Invalid credentials" });
            }
            else
            {
                return Ok(new
                {
                    Success = true,
                    Message = "Login successful",
                    Data = result
                });
            }


        }

        //forgot password for admin
        [HttpPost]
        [Route("adminForgotPassword")]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            try
            {
                if (await manager.CheckEmailExistAsync(Email))
                {
                    ForgotPasswordModel forgotPasswordModel =  await manager.ForgotPassword(Email);

                    Send send = new Send();
                    send.SendingMail(forgotPasswordModel.Email, forgotPasswordModel.Token);
                    return Ok(new ResponseModel<string> 
                    { 
                        Success = true, 
                        Message = "Mail send Successfully" 
                    });
                }
                else
                {

                    return BadRequest(new ResponseModel<string>() 
                    { 
                        Success = false, 
                        Message = "Email not send " 
                    });

                }
            }
            catch (Exception e)
            {
                throw e;

            }
        }

        //Reset Password API for admin
        [Authorize]
        [HttpPost]
        [Route("adminsResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            try
            {
                string email = User.FindFirst("EmailId").Value;
                if (await manager.ResetPassword(email, model))
                {
                    return Ok(new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Done, Password is Reset !"

                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Resetting Password Failed !!!!!"

                    });
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
