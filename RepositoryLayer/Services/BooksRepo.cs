﻿using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class BooksRepo : IBooksRepo
    {
        //dependency
        private readonly BookDBContext context;

        //access the current HTTP context (request info, headers, user claims, etc.) 
        private readonly IHttpContextAccessor httpContextAccessor;

        private readonly IDistributedCache distributedCache;

        public BooksRepo(BookDBContext context, IHttpContextAccessor httpContextAccessor, IDistributedCache distributedCache)
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
            this.distributedCache = distributedCache;
        }


        /*
         //to get all books
        public async Task<IEnumerable<BookResponseModel>> GetAllBooksAsync()
        {
            return await context.Books
                .Select(b => new BookResponseModel
                {
                    BookId = b.BookId,
                    BookName = b.BookName,
                    Author = b.Author,
                    Description = b.Description,
                    Price = b.Price,
                    DiscountPrice = b.DiscountPrice,
                    Quantity = b.Quantity,
                    BookImage = b.BookImage
                })
                .ToListAsync();
        }
        */

        //to get all books using cache
        public async Task<IEnumerable<BookResponseModel>> GetAllBooksAsync()
        {
            string cacheKey = "all_books";
            string serializedBooks;

            //to get data from Redis
            var cachedBooks = await distributedCache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedBooks))
            {
                return System.Text.Json.JsonSerializer.Deserialize<IEnumerable<BookResponseModel>>(cachedBooks);
            }

            // Cache miss – fetch from DB
            var books = await context.Books
                .Select(b => new BookResponseModel
                {
                    BookId = b.BookId,
                    BookName = b.BookName,
                    Author = b.Author,
                    Description = b.Description,
                    Price = b.Price,
                    DiscountPrice = b.DiscountPrice,
                    Quantity = b.Quantity,
                    BookImage = b.BookImage
                })
                .ToListAsync();

            // Store in Redis
            serializedBooks = System.Text.Json.JsonSerializer.Serialize(books);

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) // Cache timeout
            };

            await distributedCache.SetStringAsync(cacheKey, serializedBooks, options);

            return books;
        }



        //to get book by id
        public async Task<BookResponseModel> GetBookByIdAsync(int id)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null)
            {
                return null;
            }

            return new BookResponseModel
            {
                BookId = book.BookId,
                BookName = book.BookName,
                Author = book.Author,
                Description = book.Description,
                Price = book.Price,
                DiscountPrice = book.DiscountPrice,
                Quantity = book.Quantity,
                BookImage = book.BookImage
            };
        }


        //update book
        public async Task<bool> UpdateBookAsync(int id, BookModel updatedBook)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null) return false;

            //to get AdminId from token
            var userId = httpContextAccessor.HttpContext?.User.FindFirst("UserId")?.Value;
            var adminId = Convert.ToInt32(userId);


            book.BookName = updatedBook.BookName;
            book.Author = updatedBook.Author;
            book.Description = updatedBook.Description;
            book.Price = updatedBook.Price;
            book.DiscountPrice = updatedBook.DiscountPrice;
            book.Quantity = updatedBook.Quantity;
            book.BookImage = updatedBook.BookImage;

            book.AdminId = adminId;
            book.UpdatedAt = DateTime.Now;


            await context.SaveChangesAsync();

            // Invalidate Redis cache
            await distributedCache.RemoveAsync("all_books");

            return true;
        }



        //delete book
        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null)
            {
                return false;
            }

            context.Books.Remove(book);
            await context.SaveChangesAsync();

            // Invalidate Redis cache
            await distributedCache.RemoveAsync("all_books");
            return true;
        }

        // sort books by price in ascending order 
        public async Task<IEnumerable<BookResponseModel>> GetBooksByPriceAscAsync()
        {
            return await context.Books
               .OrderBy(b => b.Price)
               .Select(b => new BookResponseModel
               {
                   BookId = b.BookId,
                   BookName = b.BookName,
                   Author = b.Author,
                   Description = b.Description,
                   Price = b.Price,
                   DiscountPrice = b.DiscountPrice,
                   Quantity = b.Quantity,
                   BookImage = b.BookImage
               })
               .ToListAsync();
        }

        // sort books by price in descending order 
        public async Task<IEnumerable<BookResponseModel>> GetBooksByPriceDescAsync()
        {
            return await context.Books
               .OrderByDescending(b => b.Price)
               .Select(b => new BookResponseModel
               {
                   BookId = b.BookId,
                   BookName = b.BookName,
                   Author = b.Author,
                   Description = b.Description,
                   Price = b.Price,
                   DiscountPrice = b.DiscountPrice,
                   Quantity = b.Quantity,
                   BookImage = b.BookImage
               })
               .ToListAsync();
        }

        //search books by book name, author name , or recently added books
        public async Task<IEnumerable<BookResponseModel>> SearchBooksAsync(string keyword)
        {
            keyword = keyword.ToLower();

            if (keyword == "recent" || keyword == "latest" || keyword == "recent book" || keyword == "latest book")
            {
                return await context.Books
                    .OrderByDescending(b => b.CreatedAt)
                    .Take(5)
                    .Select(b => new BookResponseModel
                    {
                        BookId = b.BookId,
                        BookName = b.BookName,
                        Author = b.Author,
                        Description = b.Description,
                        Price = b.Price,
                        DiscountPrice = b.DiscountPrice,
                        Quantity = b.Quantity,
                        BookImage = b.BookImage
                    })
                    .ToListAsync();
            }

            return await context.Books
                .Where(b => b.BookName.ToLower().Contains(keyword) ||  b.Author.ToLower().Contains(keyword))
                .Select(b => new BookResponseModel
                {
                    BookId = b.BookId,
                    BookName = b.BookName,
                    Author = b.Author,
                    Description = b.Description,
                    Price = b.Price,
                    DiscountPrice = b.DiscountPrice,
                    Quantity = b.Quantity,
                    BookImage = b.BookImage
                })
                .ToListAsync();
        }



    }
}
