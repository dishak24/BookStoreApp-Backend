using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using System.Threading.Tasks;

namespace BookStoreApp.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //Dependency
        private readonly ICustomerDetailsManager manager;

        public CustomersController(ICustomerDetailsManager manager)
        {
            this.manager = manager;
        }

        //add customer details
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> AddCustomerDetailsAsync([FromBody] CustomerDetailsModel model)
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);

            var result = await manager.AddCustomerDetailsAsync(userId, model);

            if (result == null)
            {
                return BadRequest(new ResponseModel<string>
                { 
                    Success = false, 
                    Message = "Failed to add details" 
                });
            }

            return Ok(new ResponseModel<CustomerDetailsResponseModel>
            { 
                Success = true, 
                Message = "Customer details added", 
                Data = result 
            });
        }

        //get customer details
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> GetCustomerDetailsAsync()
        {
            int userId = int.Parse(User.FindFirst("UserId").Value);

            var result = await manager.GetCustomerDetailsAsync(userId);
            if (result == null)
            {
                return NotFound(new 
                { 
                    Success = false, 
                    Message = "Customer details not found" 
                });
            }

            var response = new CustomerDetailsResponseModel
            {
                CustomerId = result.CustomerId,
                FullName = result.FullName,
                Mobile = result.Mobile,
                Address = result.Address,
                City = result.City,
                State = result.State,
                Type = result.Type
            };

            return Ok(new ResponseModel<CustomerDetailsResponseModel>
            { 
                Success = true, 
                Message = "Customer details fetched", 
                Data = response 
            });
        }
    }
}
