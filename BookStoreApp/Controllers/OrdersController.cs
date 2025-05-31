using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using ManagerLayer.Interfaces;
using RepositoryLayer.Models;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace BookStoreApp.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        //dependency
        private readonly IOrdersManager manager;

        //For Logger
        private readonly ILogger<OrdersController> logger;

        public OrdersController(IOrdersManager manager, ILogger<OrdersController> logger)
        {
            this.manager = manager;
            this.logger = logger;
        }

        //to place order
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> PlaceOrder()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var orders = await manager.PlaceOrderAsync(userId);

                if (orders == null || !orders.Any())
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "No items found in the cart to place an order.",
                        Data = (object)null
                    });
                }

                var responseData = orders.Select(o => new
                {
                    o.OrderId,
                    o.BookId,
                    o.Quantity,
                    o.TotalAmount,
                    o.OrderedAt
                });

                return Ok(new
                {
                    Success = true,
                    Message = "Order placed successfully.",
                    Data = responseData
                });
            }
            catch (Exception ex)
            {
                //logger
                logger.LogError(ex.ToString());

                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.Message,
                    Data = (object)null
                });
            }
        }


        //to get all orders
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {

            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var orders = await manager.GetUserOrdersAsync(userId);

                if (orders.Count == 0)
                {
                    return Ok(new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Order Summary is Empty! Please order books.",
                        Data = null
                    });
                }

                return Ok(new 
                { 
                    Success = true, 
                    Message = "Orders retrieved successfully.", 
                    Data = orders 
                });

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
