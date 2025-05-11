using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class CartResponseModel
    {        
        public int CartId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }
        public int TotalPrice => UnitPrice * Quantity;
        public string BookImage { get; set; }
        

    }
}
