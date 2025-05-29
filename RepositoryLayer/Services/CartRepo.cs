using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Helpers;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                throw new InvalidOperationException("Book not found.");
            }

            if (book.Quantity <= 0)
            {
                throw new InvalidOperationException("Book is out of stock.");
            }

            // Check if the book is already in the cart
            var existingCartItem = await context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.BookId == bookId);

            if (existingCartItem != null)
            {
                int totalRequestedQuantity = existingCartItem.Quantity + quantity;

                if (totalRequestedQuantity > book.Quantity)
                {
                    throw new InvalidOperationException($"Only {book.Quantity - existingCartItem.Quantity} more unit(s) available in stock.");
                }

                // Update the quantity
                // Force reset to desired quantity
                existingCartItem.Quantity = quantity;
            }
            else
            {
                if (quantity > book.Quantity)
                {
                    throw new InvalidOperationException($"Only {book.Quantity} unit(s) available in stock.");
                }

                /*
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
                */

                // 3. Call the stored procedure to insert/update the cart
                await context.Database.ExecuteSqlRawAsync(
                    "EXEC SP_AddBookToCart @userId = {0}, @bookId = {1}, @quantity = {2}",
                    userId, bookId, quantity);

            }//else close

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
                Author = cartItem.Books?.Author,
                Quantity = cartItem.Quantity,
                UnitPrice = (int)cartItem.UnitPrice,
                BookImage = cartItem.Books?.BookImage,
                IsPurchased = cartItem.IsPurchased
            };

            return responseModel;
        }



        //get all cart items
        public async Task<CartListResponseModel> GetCartAsync(int userId)
        {
            /*var items = await context.Carts
                .Where(c => c.UserId == userId && !c.IsPurchased)
                .Include(c => c.Books)
                .Select(c => new CartResponseModel
                {
                    CartId = c.CartId,
                    BookId = c.BookId,
                    BookName = c.Books.BookName,
                    Author = c.Books.Author,
                    Quantity = c.Quantity,
                    UnitPrice = c.UnitPrice,
                    BookImage = c.Books.BookImage,
                    IsPurchased = c.IsPurchased
                })
                .ToListAsync();*/

            // call SP -> map to CartResponseModel
            var items = await context.CartView
                        .FromSqlRaw("EXEC SP_GetAllCarts @UserId = {0}", userId)
                        .ToListAsync();

            var result = items.Select(c => new CartResponseModel
            {
                CartId = c.CartId,
                BookId = c.BookId,
                BookName = c.BookName,
                Author = c.Author,
                Quantity = c.Quantity,
                UnitPrice = c.UnitPrice,
                BookImage = c.BookImage,
                IsPurchased = c.IsPurchased
            }).ToList();

            return new CartListResponseModel
            {
                //Items = items,
                Items = result,
                TotalAmount = items.Sum(x => x.UnitPrice * x.Quantity)
            };
        }


        //remove item from cart
        public async Task<bool> RemoveCartItemAsync(int bookId, int userId)
        {
            /*var cartItem = await context.Carts
                .Include(c => c.Books)
                .FirstOrDefaultAsync(c => c.BookId == bookId && c.UserId == userId);

            if (cartItem == null)
            {
                return false;
            }                

            context.Carts.Remove(cartItem);
            await context.SaveChangesAsync();
            return true;*/

            //updated via SP 
            var result = await context.Database.ExecuteSqlRawAsync
                (
                "EXEC SP_RemoveCartItem @userId = {0}, @bookId = {1}", userId, bookId
                );

            // result will be the number of affected rows (ideally 1 if deleted)
            return result > 0;
        }

        //update quantity of item from cart
        public async Task<CartResponseModel> UpdateCartQuantityAsync(int userId, int bookId, int quantity)
        {
            // Update via stored procedure
            await context.Database.ExecuteSqlRawAsync(
                "EXEC SP_UpdateCartQuantity @userId = {0}, @bookId = {1}, @quantity = {2}",
                userId, bookId, quantity
);


            // Fetch updated item to return
            var cartItem = await context.Carts
                .Include(c => c.Books)
                .FirstOrDefaultAsync(c => c.BookId == bookId && c.UserId == userId); 

            if (cartItem == null)
                return null;

            /*
            cartItem.Quantity = quantity;

            // Save the changes
            await context.SaveChangesAsync();
            */

            return new CartResponseModel
            {
                CartId = cartItem.CartId,
                BookId = cartItem.BookId,
                BookName = cartItem.Books.BookName,
                Author = cartItem.Books.Author,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.UnitPrice,
                BookImage = cartItem.Books.BookImage,
                IsPurchased = cartItem.IsPurchased
            };
        }



    }
}
