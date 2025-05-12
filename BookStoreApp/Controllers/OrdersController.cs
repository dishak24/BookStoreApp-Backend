using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using ManagerLayer.Interfaces;
using RepositoryLayer.Models;
using System.Linq;

namespace BookStoreApp.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        //dependency
        private readonly IOrdersManager manager;

        public OrdersController(IOrdersManager manager)
        {
            this.manager = manager;
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
                return StatusCode(500, new
                {
                    Success = false,
                    Message = ex.Message,
                    Data = (object)null
                });
            }
        }


    }
}
