using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Services
{
    public class CartManager : ICartManager
    {
        //Dependency
        private readonly ICartRepo cartRepo;

        public CartManager(ICartRepo cartRepo)
        {
            this.cartRepo = cartRepo;
        }


        //add book to cart
        public async Task<CartResponseModel> AddBookToCartAsync(int userId, int bookId, int quantity)
        {
            return await cartRepo.AddBookToCartAsync(userId, bookId, quantity);
        }

        //get all cart items
        public async Task<CartListResponseModel> GetCartAsync(int userId)
        {
            return await cartRepo.GetCartAsync(userId);
        }

        //remove item from cart
        public async Task<bool> RemoveCartItemAsync(int bookId, int userId)
        {
            return await cartRepo.RemoveCartItemAsync(bookId, userId);
        }
        
        //update quantity of item from cart
        public async Task<CartResponseModel> UpdateCartQuantityAsync(int userId, int bookId, int quantity)    
        {
            return await cartRepo.UpdateCartQuantityAsync(userId, bookId, quantity);
        }

    }
}
