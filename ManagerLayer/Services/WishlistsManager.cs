﻿using ManagerLayer.Interfaces;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Services
{
    public class WishlistsManager: IWishlistsManager
    {
        //dependency
        private readonly IWishlistsRepo wishlists;

        public WishlistsManager(IWishlistsRepo wishlists)
        {
            this.wishlists = wishlists;
        }

        //add book to wishlist
        public async Task<(string Status, WishlistResponseModel Data)> AddToWishlistAsync(int userId, int bookId)
        {
            return await wishlists.AddToWishlistAsync(userId, bookId);
        }

        //get all wishlists
        public async Task<List<WishlistResponseModel>> GetWishlistAsync(int userId)
        {
            return await wishlists.GetWishlistAsync(userId);
        }

        //remove book from wishlist
        public async Task<bool> RemoveFromWishlistAsync(int userId, int bookId)
        {
            return await wishlists.RemoveFromWishlistAsync(userId, bookId);
        }
    }
}
