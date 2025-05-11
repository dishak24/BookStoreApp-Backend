using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Helpers;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class CartRepo : ICartRepo
    {
        //dependencies
        private readonly BookDBContext context;
        private readonly JwtTokenManager tokenManager;

        public CartRepo(BookDBContext context, JwtTokenManager tokenManager)
        {
            this.context = context;
            this.tokenManager = tokenManager;
        }

        //add book to cart
        public async Task<CartResponseModel> AddBookToCartAsync(int userId, int bookId, int quantity)
        {
            // Get the book entity
            var book = await context.Books.FindAsync(bookId);
            if (book == null)
            {
                return null;
            }

            // Check if the book is already in the cart
            var existingCartItem = await context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.BookId == bookId);

            if (existingCartItem != null)
            {
                // Update the quantity if the book is already in the cart
                existingCartItem.Quantity += quantity;
            }
            else
            {
                // Add the new book to the cart
                var newCartItem = new CartEntity
                {
                    UserId = userId,
                    BookId = bookId,
                    Quantity = quantity,
                    UnitPrice = book.DiscountPrice,
                    AddedAt = DateTime.Now
                };
                await context.Carts.AddAsync(newCartItem);
            }

            // Save changes
            await context.SaveChangesAsync();

            // Fetch the updated cart item with book details
            var cartItem = await context.Carts
                .Include(c => c.Books)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.BookId == bookId);

            // Convert to CartResponseModel
            var responseModel = new CartResponseModel
            {
                CartId = cartItem.CartId,
                BookId = cartItem.BookId,
                BookName = cartItem.Books?.BookName,
                Author = cartItem.Books.Author,
                Quantity = cartItem.Quantity,
                UnitPrice = (int)cartItem.UnitPrice,
                BookImage = cartItem.Books?.BookImage
            };

            return responseModel;
        }


        //get all cart items
        public async Task<List<CartResponseModel>> GetCartAsync(int userId)
        {
            return await context.Carts
                .Where(c => c.UserId == userId)
                .Include(c => c.Books)  // to include related Books data
                .Select(c => new CartResponseModel
                {
                    CartId = c.CartId,
                    BookId = c.BookId,
                    BookName = c.Books.BookName,
                    Author = c.Books.Author,
                    Quantity = c.Quantity,
                    UnitPrice = c.UnitPrice,
                    BookImage = c.Books.BookImage
                })
                .ToListAsync();
        }

        //remove item from cart
        public async Task<bool> RemoveCartItemAsync(int cartId, int userId)
        {
            var cartItem = await context.Carts
                .FirstOrDefaultAsync(c => c.CartId == cartId && c.UserId == userId);

            if (cartItem == null)
            {
                return false;
            }                

            context.Carts.Remove(cartItem);
            await context.SaveChangesAsync();
            return true;
        }


    }
}
