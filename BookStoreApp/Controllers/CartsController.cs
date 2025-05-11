using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entity;
using RepositoryLayer.Migrations;
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
        [Authorize(Roles = "User")]
        [HttpPost]        
        public async Task<IActionResult> AddBookToCart(int bookId)
        {
            try
            {
                // Extract the UserId from the token claims
                int userId = int.Parse(User.FindFirst("UserId").Value);
                int quantity = 1;

                var cart = await manager.AddBookToCartAsync(userId, bookId, quantity);
                if (cart == null)
                {
                    return NotFound(new ResponseModel<CartResponseModel>
                    {
                        Success = false,
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

        //get all cart items
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                // Extract UserId from JWT token
                int userId = int.Parse(User.FindFirst("UserId").Value);

                // Get the cart items for the current user
                var result = await manager.GetCartAsync(userId);

                if (result == null)
                {
                    return NotFound(new ResponseModel<List<CartResponseModel>>
                    {
                        Success = false,
                        Message = "Cart items not found !!!",
                        Data = result
                    });
                }
                else
                {
                    return Ok(new ResponseModel<List<CartResponseModel>>
                    {
                        Success = true,
                        Message = "Successfully getting cart items.",
                        Data = result
                    });
                }
                    
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An internal error occurred. Please try again later !!",
                    Data = e.Message
                });

            }
            
        }

        //remove item from cart
        [Authorize(Roles = "User")]
        [HttpDelete("{cartId}")]
        public async Task<IActionResult> RemoveCartItem(int cartId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);

                var result = await manager.RemoveCartItemAsync(cartId, userId);

                if (!result)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Cart item not found !!!",
                        Data = (object)null
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Message = "Cart item removed successfully.",
                    Data = new { CartId = cartId }
                });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An internal error occurred. Please try again later !!",
                    Data = e.Message
                });

            }

        }

        //update quantity of item from cart
        [Authorize(Roles = "User")]
        [HttpPut("{cartId}")]
        public async Task<IActionResult> UpdateCartQuantity(int cartId, [FromBody] UpdateQuantityModel model)
        {
            try
            {
                if (model.Quantity <= 0)
                {
                    return BadRequest(new
                    {
                        Success = false,
                        Message = "Quantity must be greater than 0",
                        Data = (object)null
                    });
                }

                int userId = int.Parse(User.FindFirst("UserId").Value);

                var result = await manager.UpdateCartQuantityAsync(cartId, userId, model.Quantity);

                if (result == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Cart item not found !!!",
                        Data = (object)null
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Message = "Cart quantity updated successfully",
                    Data = result
                });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An internal error occurred. Please try again later !!",
                    Data = e.Message
                });

            }


        }


    }
}
