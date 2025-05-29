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
    public class OrdersRepo : IOrdersRepo
    {
        //dependencies
        private readonly BookDBContext context;
        private readonly JwtTokenManager tokenManager;

        public OrdersRepo(BookDBContext context, JwtTokenManager tokenManager)
        {
            this.context = context;
            this.tokenManager = tokenManager;
        }

        //to place order
        public async Task<List<OrderSummaryEntity>> PlaceOrderAsync(int userId)
        {
            var cartItems = await context.Carts
                .Include(c => c.Books)
                .Where(c => c.UserId == userId && c.IsPurchased == false)
                .ToListAsync();

            if (!cartItems.Any())
                return null;

            var orderSummaries = new List<OrderSummaryEntity>();

            foreach (var item in cartItems)
            {
                if (item.Books == null)
                    throw new Exception($"Book with ID {item.BookId} not found.");

                if (item.Quantity > item.Books.Quantity)
                    throw new Exception($"Insufficient stock for book: {item.Books.BookName}");

                // Set as purchased
                item.IsPurchased = true;

                // Add to order
                var order = new OrderSummaryEntity
                {
                    UserId = item.UserId,
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    TotalAmount = item.UnitPrice * item.Quantity,
                    OrderedAt = DateTime.Now
                };
                await context.Orders.AddAsync(order);
                orderSummaries.Add(order);

                // Update book stock
                item.Books.Quantity -= item.Quantity;
            }

            // Remove purchased items from cart
            context.Carts.RemoveRange(cartItems);
            await context.SaveChangesAsync();

            return orderSummaries;
        }


        //get all orders
        public async Task<List<OrderResponseModel>> GetUserOrdersAsync(int userId)
        {
            return await context.Orders
                .Include(o => o.Books)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderedAt) // Sort by date DESCENDING
                .Select(o => new OrderResponseModel
                {
                    OrderId = o.OrderId,
                    BookId = o.BookId,
                    BookName = o.Books.BookName,
                    Author = o.Books.Author,
                    Quantity = o.Quantity,
                    TotalAmount = o.TotalAmount,
                    BookImage = o.Books.BookImage,
                    OrderedAt = o.OrderedAt
                })
                .ToListAsync();
        }



    }
}
