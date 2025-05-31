using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStoreApp.Controllers
{
    [Route("api/wishlists")]
    [ApiController]
    public class WishlistsController : ControllerBase
    {
        //dependency
        private readonly IWishlistsManager manager;

        //For Logger
        private readonly ILogger<WishlistsController> logger;

        public WishlistsController(IWishlistsManager manager, ILogger<WishlistsController> logger)
        {
            this.manager = manager;
            this.logger = logger;
        }

        //add book to wishlist
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> AddToWishlist(int bookId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);

                var (status, data) = await manager.AddToWishlistAsync(userId, bookId);

                return status switch
                {
                    "BookNotFound" => NotFound(new ResponseModel<string>
                    { 
                        Success = false, 
                        Message = "Book not found", 
                        Data = null 
                    }),
                    "AlreadyExists" => Conflict(new ResponseModel<string>
                    { 
                        Success = false, 
                        Message = "Book is already in the wishlist", 
                        Data = null
                    }),
                    "Success" => Ok(new  ResponseModel<WishlistResponseModel>
                    { 
                        Success = true, 
                        Message = "Book added to wishlist", 
                        Data = data 
                    })
                   
                };
            }
            catch (Exception e)
            {
                //logger
                logger.LogError(e.ToString());

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An internal error occurred. Please try again later !!",
                    Data = e.Message
                });

            }

        }

        //get all wishlists
        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> GetWishlist()
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var list = await manager.GetWishlistAsync(userId);

                if (list.Count == 0)
                {
                    return Ok(new ResponseModel<string>
                    {
                        Success = true,
                        Message = "Wishlist is Empty! Please add book to wishlist.",
                        Data = null
                    });
                }

                return Ok(new ResponseModel<List<WishlistResponseModel>>
                { 
                    Success = true, 
                    Message = "Getting Wishlist Successfully.", 
                    Data = list 
                });
            }
            catch (Exception e)
            {
                //logger
                logger.LogError(e.ToString());

                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An internal error occurred. Please try again later !!",
                    Data = e.Message
                });

            }
            
        }

        //remove book from wishlist
        [Authorize(Roles = "User")]
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> RemoveFromWishlist(int bookId)
        {
            try
            {
                int userId = int.Parse(User.FindFirst("UserId").Value);
                var removed = await manager.RemoveFromWishlistAsync(userId, bookId);

                if (!removed)
                {
                    return NotFound(new ResponseModel<string>
                    { 
                        Success = false, 
                        Message = "Book not found in wishlist !!", 
                        Data = null 
                    });
                }
                    

                return Ok(new ResponseModel<string>
                { 
                    Success = true, 
                    Message = "Book removed from wishlist.", 
                    Data = null 
                });

            }
            catch (Exception e)
            {
                //logger
                logger.LogError(e.ToString());

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

