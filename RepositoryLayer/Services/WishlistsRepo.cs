using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    public class WishlistsRepo: IWishlistsRepo
    {
        //dependency
        private readonly BookDBContext context;

        //access the current HTTP context (request info, headers, user claims, etc.) 
        private readonly IHttpContextAccessor httpContextAccessor;
        public WishlistsRepo(BookDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
        }

        //add book to wishlist
        public async Task<(string Status, WishlistResponseModel Data)> AddToWishlistAsync(int userId, int bookId)
        {
            var book = await context.Books.FindAsync(bookId);
            if (book == null)
            {
                return ("BookNotFound", null);
            }            

            var exists = await context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.BookId == bookId);

            if (exists != null)
            {
                return ("AlreadyExists", null);
            }                

            var wishlist = new WishlistEntity
            {
                UserId = userId,
                BookId = bookId,
                AddedAt = DateTime.Now
            };

            await context.Wishlists.AddAsync(wishlist);
            await context.SaveChangesAsync();

            var data = new WishlistResponseModel
            {
                WishlistId = wishlist.WishlistId,
                BookId = wishlist.BookId,
                BookName = book.BookName,
                Author = book.Author,
                BookImage = book.BookImage,
                Price = book.Price,
                DiscountPrice = book.DiscountPrice
               
            };

            return ("Success", data);
        }

        //get all wishlists
        public async Task<List<WishlistResponseModel>> GetWishlistAsync(int userId)
        {
            return await context.Wishlists
                .Include(w => w.Book)
                .Where(w => w.UserId == userId)
                .Select(w => new WishlistResponseModel
                {
                    WishlistId = w.WishlistId,
                    BookId = w.BookId,
                    BookName = w.Book.BookName,
                    Author = w.Book.Author,
                    BookImage = w.Book.BookImage,
                    Price = w.Book.Price,
                    DiscountPrice = w.Book.DiscountPrice
                })
                .ToListAsync();
        }


    }
}
