using CommonLayer.Model;
using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using System.Threading.Tasks;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserManager userManager;
        private readonly IJwtTokenManager tokenManager;

        public UserController(IUserManager userManager , IJwtTokenManager tokenManager)
        {
            this.userManager = userManager;
            this.tokenManager = tokenManager;
        }

        //User registration
        [HttpPost]
        [Route("userRegister")]
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
        [HttpPost()]
        [Route("userLogin")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await userManager.LoginAsync(model);
            if (user == null)
            {
                return Unauthorized();
            }

            var token = await tokenManager.GenerateToken(new JwtModel
            {
                Id = user.UserId,
                Email = user.Email,
                Role = "User"
            });

            return Ok(new { Token = token });
        }

    }
}
