using BookStoreApp.filters;
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
    //[RateLimit(10, 60)] // applies to all actions in this controller
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
                        Message = "Book not found!",
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
            catch (InvalidOperationException ex)
            {
                // This handles insufficient stock, book already added, or other validation issues
                return BadRequest(new ResponseModel<string>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                // Handles all other unexpected errors
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An internal error occurred. Please try again later.",
                    Data = ex.Message
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
                    return NotFound(new ResponseModel<CartListResponseModel>
                    {
                        Success = false,
                        Message = "Cart items not found !!!",
                        Data = result
                    });
                }
                else
                {
                    if (result.Items.Count == 0)
                    {
                        return Ok(new ResponseModel<CartListResponseModel>
                        {
                            Success = true,
                            Message = "Cart is Empty! Please add book to cart.",
                            Data = null
                        });
                    }
                    else
                    {
                        return Ok(new ResponseModel<CartListResponseModel>
                        {
                            Success = true,
                            Message = "Successfully getting cart items.",
                            Data = result
                        });
                    }
                        
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
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> RemoveCartItem(int bookId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);

                var result = await manager.RemoveCartItemAsync(bookId, userId);

                if (!result)
                {
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Cart item not found !!!",
                        Data = null
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Message = "Cart item removed successfully.",
                    Data = new { BookId = bookId }
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
        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateCartQuantity(int bookId, [FromBody] UpdateQuantityModel model)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                if (model.Quantity == 0)
                {
                    var isRemoved = await manager.RemoveCartItemAsync(bookId, userId);

                    if (!isRemoved)
                    {
                        return NotFound(new ResponseModel<string>
                        {
                            Success = false,
                            Message = "Book not found in cart!!",
                            Data = null
                        });
                    }

                    return Ok(new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Book removed from cart because quantity was set to 0.",
                        Data = null
                    });
                }

                

                var result = await manager.UpdateCartQuantityAsync(userId, bookId,  model.Quantity);

                if (result == null)
                {
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Book not found !!!",
                        Data = null
                    });
                }

                return Ok(new
                {
                    Success = true,
                    Message = "Books quantity updated successfully",
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
