using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Services
{
    public class OrdersManager : IOrdersManager
    {
        //dependency
        private readonly IOrdersRepo orderRepo;

        public OrdersManager(IOrdersRepo orderRepo)
        {
            this.orderRepo = orderRepo;
        }

        //to place order
        public async Task<List<OrderSummaryEntity>> PlaceOrderAsync(int userId)
        {
            return await orderRepo.PlaceOrderAsync(userId);
        }
    }
}
