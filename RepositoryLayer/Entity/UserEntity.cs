﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Role { get; set; }


        // for refresh token
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        // for cart 
        public virtual ICollection<CartEntity> Carts { get; set; }

        //for wishlist
        public virtual ICollection<WishlistEntity> Wishlists { get; set; }

        //for orders
        public virtual ICollection<OrderSummaryEntity> Orders { get; set; }

        //for customer details
        public virtual ICollection<CustomerDetailsEntity> CustomerDetails { get; set; }


    }
}
