﻿using RepositoryLayer.Entity;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interfaces
{
    public interface ICartManager
    {
        //add book to cart
        public Task<CartResponseModel> AddBookToCartAsync(int userId, int bookId, int quantity);

        //get all cart items
        public Task<CartListResponseModel> GetCartAsync(int userId);

        //remove item from cart
        public Task<bool> RemoveCartItemAsync(int bookId, int userId);

        //update quantity of item from cart
        public Task<CartResponseModel> UpdateCartQuantityAsync(int userId, int bookId, int quantity);
    }

}
