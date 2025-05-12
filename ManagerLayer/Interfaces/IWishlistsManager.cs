using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interfaces
{
    public interface IWishlistsManager
    {
        //add book to wishlist
        public Task<(string Status, WishlistResponseModel Data)> AddToWishlistAsync(int userId, int bookId);

        //get all wishlists
        public Task<List<WishlistResponseModel>> GetWishlistAsync(int userId);

        //remove book from wishlist
        public Task<bool> RemoveFromWishlistAsync(int userId, int bookId);
    }

}
