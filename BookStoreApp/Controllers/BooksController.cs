﻿using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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

        //For Logger
        private readonly ILogger<BooksController> logger;

        public BooksController(IBooksManager booksManager, ILogger<BooksController> logger)
        {
            this.booksManager = booksManager;
            this.logger = logger;
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
                    return Ok(new ResponseModel<IEnumerable<BookResponseModel>>
                    {
                        Success = true,
                        Message = "Got All Books Successfully",
                        Data = books
                    });
                }
                else
                {
                    return BadRequest(new ResponseModel<IEnumerable<BookResponseModel>>
                    {
                        Success = false,
                        Message = "Failed to Get All Books !!",
                        Data = books
                    });
                }
                
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
                    return NotFound(new ResponseModel<BookResponseModel>
                    {
                        Success = false,
                        Message = "Book Id not found !!!",
                        Data = book
                    });
                }
                else
                {

                    return Ok(new ResponseModel<BookResponseModel>
                    {
                        Success = true,
                        Message = "Got Book details Successfully",
                        Data = book
                    });
                }                
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

        // sort by price: Ascending
        [Authorize(Roles = "Admin,User")]
        [HttpGet("sort")]
        public async Task<IActionResult> GetBooksByPriceAscAsync()
        {
            try
            {
                var books = await booksManager.GetBooksByPriceAscAsync();
                if (books != null)
                {
                    return Ok(new ResponseModel<IEnumerable<BookResponseModel>>
                    {
                        Success = true,
                        Message = "Sorted books in ascending order.",
                        Data = books
                    });
                }
                else
                {
                    return NotFound(new ResponseModel<IEnumerable<BookResponseModel>>
                    {
                        Success = false,
                        Message = "Failed to Sort !!",
                        Data = books
                    });
                }

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

        // sort by price: Descending
        [Authorize(Roles = "Admin,User")]
        [HttpGet("sort/desc")]
        public async Task<IActionResult> GetBooksByPriceDescAsync()
        {
            try
            {
                var books = await booksManager.GetBooksByPriceDescAsync();
                if (books != null)
                {
                    return Ok(new ResponseModel<IEnumerable<BookResponseModel>>
                    {
                        Success = true,
                        Message = "Sorted books in Descending order.",
                        Data = books
                    });
                }
                else
                {
                    return NotFound(new ResponseModel<IEnumerable<BookResponseModel>>
                    {
                        Success = false,
                        Message = "Failed to Sort !!",
                        Data = books
                    });
                }

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

        // search books
        [Authorize(Roles = "Admin,User")]
        [HttpGet("search")]        
        public async Task<IActionResult> SearchBooksAsync([FromQuery] string keyword)
        {
            try
            {
                var books = await booksManager.SearchBooksAsync(keyword);
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    return BadRequest("Keyword cannot be empty !!");
                }
                   
                if (books != null)
                {
                    return Ok(new ResponseModel<IEnumerable<BookResponseModel>>
                    {
                        Success = true,
                        Message = "successfully getting searched books.",
                        Data = books
                    });
                }
                else
                {
                    return NotFound(new ResponseModel<IEnumerable<BookResponseModel>>
                    {
                        Success = false,
                        Message = "Failed to search !!",
                        Data = books
                    });
                }

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
