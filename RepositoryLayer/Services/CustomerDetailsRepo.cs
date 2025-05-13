using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Helpers;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class CustomerDetailsRepo : ICustomerDetailsRepo
    {
        //dependencies
        private readonly BookDBContext context;
        private readonly JwtTokenManager tokenManager;

        public CustomerDetailsRepo(BookDBContext context, JwtTokenManager tokenManager)
        {
            this.context = context;
            this.tokenManager = tokenManager;
        }

        //add customer details
        public async Task<CustomerDetailsResponseModel> AddCustomerDetailsAsync(int userId, CustomerDetailsModel model)
        {
            var customer = new CustomerDetailsEntity
            {
                UserId = userId,
                FullName = model.FullName,
                Mobile = model.Mobile,
                Address = model.Address,
                City = model.City,
                State = model.State,
                Type = model.Type
            };

            context.CustomerDetails.Add(customer);
            await context.SaveChangesAsync();

            var response = new CustomerDetailsResponseModel
            {
                CustomerId = customer.CustomerId,
                FullName = customer.FullName,
                Mobile = customer.Mobile,
                Address = customer.Address,
                City = customer.City,
                State = customer.State,
                Type = customer.Type
            };

            return response;
        }


        //get customer details
        public async Task<CustomerDetailsEntity> GetCustomerDetailsAsync(int userId)
        {
            return await context.CustomerDetails.FirstOrDefaultAsync(c => c.UserId == userId);
        }

    }
}
