using ManagerLayer.Interfaces;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Services
{
    public class BooksManager : IBooksManager
    {
        //dependency
        private readonly IBooksRepo bookRepo;

        public BooksManager(IBooksRepo bookRepo)
        {
            this.bookRepo = bookRepo;
        }

        //to get all books
        public async Task<IEnumerable<Books>> GetAllBooksAsync()
        {
            return await bookRepo.GetAllBooksAsync();
        }

        //to get book by id
        public async Task<Books> GetBookByIdAsync(int id)
        {
            return await bookRepo.GetBookByIdAsync(id);
        }

        //update book
        public async Task<bool> UpdateBookAsync(int id, Books book)
        {
            return await bookRepo.UpdateBookAsync(id, book);
        }

        //delete book

        public async Task<bool> DeleteBookAsync(int id)
        {
            return await bookRepo.DeleteBookAsync(id);
        }
    }
}
