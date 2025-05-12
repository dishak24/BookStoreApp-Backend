using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class OrderResponseModel
    {
        
        public int OrderId { get; set; }
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public int Quantity { get; set; }
        public int TotalAmount { get; set; }
        public string BookImage { get; set; }
        public DateTime OrderedAt { get; set; }
        

    }
}
