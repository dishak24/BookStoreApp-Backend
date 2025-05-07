
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Helpers;
using RepositoryLayer.Models;
using RepositoryLayer.Services;
using System;
using System.Threading.Tasks;

namespace BookStoreApp.Controllers
{
    //[Route("api/[controller]")]
   // [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;
        

        public UserController(IUserManager userManager)
        {
            this.userManager = userManager;
            
        }

        //User registration
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            if (model == null)
            {
                return BadRequest("data is null");
            }
            // to checking email already used or not
            var check = await userManager.CheckEmailExistAsync(model.Email);

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
                var result = await userManager.RegisterAsync(model);

                if (result != null)
                {
                    return Ok(new ResponseModel<UserEntity>
                    {
                        Success = true,
                        Message = "Registration Successfull !",
                        Data = result
                    });
                }
                return BadRequest(new ResponseModel<UserEntity>
                {
                    Success = false,
                    Message = "Registration failed !!!!",
                    Data = result
                });
            }
        }


        //user login
        [HttpPost]
        [Route("userLogin")]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            var result = await userManager.LoginAsync(model);

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

        //forgot password for user
        [HttpPost]
        [Route("userForgotPassword")]
        public async Task<IActionResult> ForgotPasswordAsync(string Email)
        {
            try
            {
                if (await userManager.CheckEmailExistAsync(Email))
                {
                    ForgotPasswordModel forgotPasswordModel = await userManager.ForgotPasswordAsync(Email);

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

        //Reset Password API for user
        [Authorize]
        [HttpPost]
        [Route("usersResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            try
            {
                string email = User.FindFirst("EmailId").Value;
                if (await userManager.ResetPasswordAsync(email, model))
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
