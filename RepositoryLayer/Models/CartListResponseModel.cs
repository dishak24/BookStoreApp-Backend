using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Models
{
    public class CartListResponseModel
    {
        public List<CartResponseModel> Items { get; set; }
        public int TotalAmount { get; set; }
    }
}
