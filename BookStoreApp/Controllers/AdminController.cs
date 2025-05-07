using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;
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
    }
}
