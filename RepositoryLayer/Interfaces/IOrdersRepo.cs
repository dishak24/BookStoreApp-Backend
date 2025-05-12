using RepositoryLayer.Entity;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IOrdersRepo
    {
        //to place order
        public Task<List<OrderSummaryEntity>> PlaceOrderAsync(int userId);

        //get all orders
        public Task<List<OrderResponseModel>> GetUserOrdersAsync(int userId);
    }
}
