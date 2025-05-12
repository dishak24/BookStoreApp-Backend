using RepositoryLayer.Entity;
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
    }
}
