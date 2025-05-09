using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class BookModel
    {
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int DiscountPrice { get; set; }
        public int Quantity { get; set; }
        public string BookImage { get; set; }
        public int AdminId { get; set; }

     // public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
