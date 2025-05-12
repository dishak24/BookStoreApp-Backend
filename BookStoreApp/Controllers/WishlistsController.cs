using ManagerLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Models;
using System;
using System.Threading.Tasks;

namespace BookStoreApp.Controllers
{
    [Route("api/wishlists")]
    [ApiController]
    public class WishlistsController : ControllerBase
    {
        //dependency
        private readonly IWishlistsManager manager;

        public WishlistsController(IWishlistsManager manager)
        {
            this.manager = manager;
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
