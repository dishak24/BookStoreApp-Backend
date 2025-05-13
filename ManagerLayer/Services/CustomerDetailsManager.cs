using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Services
{
    public class CustomerDetailsManager : ICustomerDetailsManager
    {
        //Dependency
        private readonly ICustomerDetailsRepo repo;

        public CustomerDetailsManager(ICustomerDetailsRepo repo)
        {
            this.repo = repo;
        }

        //add customer details
        public async Task<CustomerDetailsResponseModel> AddCustomerDetailsAsync(int userId, CustomerDetailsModel model)
        {
            return await repo.AddCustomerDetailsAsync(userId, model);
        }


        //get customer details
        public async Task<CustomerDetailsEntity> GetCustomerDetailsAsync(int userId)
        {
            return await repo.GetCustomerDetailsAsync(userId);
        }
    }
}
