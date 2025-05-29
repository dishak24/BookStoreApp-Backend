using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace RepositoryLayer.Models
{
    public class CartViewModel
    {
        public int CartId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public string BookImage { get; set; }
        public bool IsPurchased { get; set; }
    }

}
