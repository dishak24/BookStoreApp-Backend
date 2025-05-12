using RepositoryLayer.Entity;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interfaces
{
    public interface IOrdersManager
    {
        //to place order
        public Task<List<OrderSummaryEntity>> PlaceOrderAsync(int userId);

        //get all orders
        public Task<List<OrderResponseModel>> GetUserOrdersAsync(int userId);
    }
}
