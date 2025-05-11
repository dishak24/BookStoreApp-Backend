using RepositoryLayer.Entity;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface ICartRepo
    {
        //add book to cart
        public Task<CartResponseModel> AddBookToCartAsync(int userId, int bookId, int quantity);


    }
}
