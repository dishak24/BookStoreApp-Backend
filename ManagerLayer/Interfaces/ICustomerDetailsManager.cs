using RepositoryLayer.Entity;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interfaces
{
    public interface ICustomerDetailsManager
    {
        //add customer details
        public Task<CustomerDetailsResponseModel> AddCustomerDetailsAsync(int userId, CustomerDetailsModel model);

        //get customer details
        public Task<CustomerDetailsEntity> GetCustomerDetailsAsync(int userId);

    }
}
