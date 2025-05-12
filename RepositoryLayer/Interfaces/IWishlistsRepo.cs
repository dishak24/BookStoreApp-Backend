using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IWishlistsRepo
    {
        //add book to wishlist
        public Task<(string Status, WishlistResponseModel Data)> AddToWishlistAsync(int userId, int bookId);

        //get all wishlists
        public Task<List<WishlistResponseModel>> GetWishlistAsync(int userId);
    }
}
