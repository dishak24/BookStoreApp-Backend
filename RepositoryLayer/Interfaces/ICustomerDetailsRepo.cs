using RepositoryLayer.Entity;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface ICustomerDetailsRepo
    {
        //add customer details
        public Task<CustomerDetailsResponseModel> AddCustomerDetailsAsync(int userId, CustomerDetailsModel model);

        //get customer details
        public Task<CustomerDetailsEntity> GetCustomerDetailsAsync(int userId);


    }
}
