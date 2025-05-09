using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class BookResponseModel
    {
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int DiscountPrice { get; set; }
        public int Quantity { get; set; }
        public string BookImage { get; set; }
    }
}
