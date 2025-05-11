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
        public async Task<List<CartResponseModel>> GetCartAsync(int userId)
        {
            return await cartRepo.GetCartAsync(userId);
        }

        //remove item from cart
        public async Task<bool> RemoveCartItemAsync(int cartId, int userId)
        {
            return await cartRepo.RemoveCartItemAsync(cartId, userId);
        }

        //update quantity of item from cart
        public async Task<CartResponseModel> UpdateCartQuantityAsync(int cartId, int userId, int quantity)
        {
            return await cartRepo.UpdateCartQuantityAsync(cartId, userId, quantity);
        }

    }
}
