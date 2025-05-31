using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Entity;
using RepositoryLayer.Helpers;
using RepositoryLayer.Models;
using System;
using System.Threading.Tasks;

namespace BookStoreApp.Controllers
{
    [Route("api/admins")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminManager manager;

        //For Logger
        private readonly ILogger<AdminController> logger;

        public AdminController(IAdminManager manager, ILogger<AdminController> logger)
        {
            this.manager = manager;
            this.logger = logger;
        }

        //admin registration
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterModel model)
        {
            try
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
                            Message = "Registered Successfully !",
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
            catch (Exception e)
            {
                //logger
                logger.LogError(e.ToString());

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An internal error occurred. Please try again later.",
                    Data = e.Message
                });

            }
            
        }


        //admin login
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            try
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
            catch (Exception e)
            {
                //logger
                logger.LogError(e.ToString());

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An internal error occurred. Please try again later.",
                    Data = e.Message
                });

            }
            

        }

        //forgot password for admin
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync(string Email)
        {
            try
            {
                if (await manager.CheckEmailExistAsync(Email))
                {
                    ForgotPasswordModel forgotPasswordModel =  await manager.ForgotPasswordAsync(Email);

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
                //logger
                logger.LogError(e.ToString());

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An internal error occurred. Please try again later.",
                    Data = e.Message
                });

            }
        }

        //Reset Password API for admin
        [Authorize]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            try
            {
                string email = User.FindFirst("EmailId").Value;
                if (await manager.ResetPasswordAsync(email, model))
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
                //logger
                logger.LogError(e.ToString());

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An internal error occurred. Please try again later.",
                    Data = e.Message
                });
            }
        }

    }
}
