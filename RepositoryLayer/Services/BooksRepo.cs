using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class BooksRepo : IBooksRepo
    {
        //dependency
        private readonly BooksContext context;
        public BooksRepo(BooksContext context)
        {
            this.context = context;
        }

        //to get all books
        public async Task<IEnumerable<Books>> GetAllBooksAsync()
        {
            return await context.Books.ToListAsync();
        }

    }
}
