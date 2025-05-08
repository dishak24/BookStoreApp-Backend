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
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        //Dependency
        private readonly IBooksManager booksManager;

        public BooksController(IBooksManager booksManager)
        {
            this.booksManager = booksManager;
        }

        // Accessible by Admin and user
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            try
            {

                // Check if user is authenticated
                if (!User.Identity.IsAuthenticated)
                {
                    return Unauthorized(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "You are not authorized! Please login to continue.",
                        
                    });
                }

                // check if user has required role
                if (!(User.IsInRole("Admin") || User.IsInRole("User")))
                {
                    return Forbid();
                }

                var books = await booksManager.GetAllBooksAsync();
                if (books != null)
                {
                    return Ok(new ResponseModel<IEnumerable<Books>>
                    {
                        Success = true,
                        Message = "Got All Books Successfully",
                        Data = books
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<IEnumerable<Books>>
                    {
                        Success = false,
                        Message = "Failed to Get All Books !!",
                        Data = books
                    });
                }
                
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
