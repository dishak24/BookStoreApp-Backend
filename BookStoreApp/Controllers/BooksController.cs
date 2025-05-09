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
using static System.Reflection.Metadata.BlobBuilder;

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

        //to get book by id
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                var book = await booksManager.GetBookByIdAsync(id);
                if (book == null)
                {
                    return NotFound(new ResponseModel<Books>
                    {
                        Success = false,
                        Message = "Book Id not found !!!",
                        Data = book
                    });
                }
                else
                {
                    return Ok(new ResponseModel<Books>
                    {
                        Success = true,
                        Message = "Got Book details Successfully",
                        Data = book
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


        // Only Admin can update
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, BookModel updatedBook)
        {
            try
            {
                var result = await booksManager.UpdateBookAsync(id, updatedBook);
                if (result)
                {
                    return Ok(new ResponseModel<bool>
                    {
                        Success = true,
                        Message = " Book details updated Successfully",
                        Data = result
                    });
                }
                else
                {
                    return NotFound(new ResponseModel<bool>
                    {
                        Success = false,
                        Message = "Book Id not found !!!",
                        Data = result
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

        // Only Admin can delete
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var result = await booksManager.DeleteBookAsync(id);

                if (result)
                {
                    return Ok(new ResponseModel<bool>
                    {
                        Success = true,
                        Message = " Book deleted Successfully",
                        Data = result
                    });
                }
                else
                {
                    return NotFound(new ResponseModel<bool>
                    {
                        Success = false,
                        Message = "Book Id not found !!!",
                        Data = result
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
