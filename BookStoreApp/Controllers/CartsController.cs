using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStoreApp.Controllers
{
    [Route("api/carts")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        //dependency
        private readonly ICartManager manager;

        public CartsController(ICartManager manager)
        {
            this.manager = manager;
        }

        // Add a book to the cart
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddBookToCart(int bookId, int quantity)
        {
            try
            {
                // Extract the UserId from the token claims
                int userId = int.Parse(User.FindFirst("UserId").Value);

                var cart = await manager.AddBookToCartAsync(userId, bookId, quantity);
                if (cart == null)
                {
                    return NotFound(new ResponseModel<CartResponseModel>
                    {
                        Success = true,
                        Message = "Book not found !!!",
                        Data = cart
                    });
                }


                return Ok(new ResponseModel<CartResponseModel>
                {
                    Success = true,
                    Message = "Successfully added book to the cart.",
                    Data = cart
                });
            }
            catch (Exception e)
            {
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
