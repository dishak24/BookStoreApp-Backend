using RepositoryLayer.Entity;
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
        public Task<List<CartResponseModel>> GetCartAsync(int userId);
    }
}
