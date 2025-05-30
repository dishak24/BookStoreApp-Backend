﻿
using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Helpers;
using RepositoryLayer.Models;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreApp.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;
        private readonly BookDBContext context;

        //For Rabbit MQ
        private readonly IBus bus;

        //For Logger
        private readonly ILogger<UserController> logger;

        public UserController(IUserManager userManager, IBus bus, BookDBContext context, ILogger<UserController> logger)
        {
            this.userManager = userManager;
            this.bus = bus;
            this.context = context;
            this.logger = logger;
            
        }

        //User registration
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


        //user login
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginModel model)
        {
            try
            {
                var result = await userManager.LoginAsync(model);

                if (result == null)
                {
                    return Unauthorized(new 
                    { 
                        Success = false, 
                        Message = "Invalid credentials" 
                    });
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

        //forgot password for user
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordAsync(string Email)
        {
            try
            {
                if (await userManager.CheckEmailExistAsync(Email))
                {
                    ForgotPasswordModel forgotPasswordModel = await userManager.ForgotPasswordAsync(Email);

                    Send send = new Send();

                    send.SendingMail(forgotPasswordModel.Email, forgotPasswordModel.Token);

                    Uri uri = new Uri("rabbitmq://localhost/BookStoreEmailQueue");
                    var endPoint = await bus.GetSendEndpoint(uri);

                    await endPoint.Send(forgotPasswordModel);

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

        //Reset Password API for user
        [Authorize]
        [HttpPost("reset-password")]
        
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


        //get names starts with s

        [HttpGet("names-starts-with-s")]

        public async Task<ActionResult<List<UserEntity>>> NamesStartWithSAsync()
        {
            var userName = await context.Users.Where(u => u.FullName.StartsWith("s")).ToListAsync();
            return Ok(userName);
        } 
    }
}
