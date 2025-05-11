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
    public class BooksManager : IBooksManager
    {
        //dependency
        private readonly IBooksRepo bookRepo;

        public BooksManager(IBooksRepo bookRepo)
        {
            this.bookRepo = bookRepo;
        }

        //to get all books
        public async Task<IEnumerable<BookResponseModel>> GetAllBooksAsync()
        {
            return await bookRepo.GetAllBooksAsync();
        }

        //to get book by id
        public async Task<BookResponseModel> GetBookByIdAsync(int id)
        {
            return await bookRepo.GetBookByIdAsync(id);
        }

        //update book
        public async Task<bool> UpdateBookAsync(int id, BookModel book)
        {
            return await bookRepo.UpdateBookAsync(id, book);
        }

        //delete book

        public async Task<bool> DeleteBookAsync(int id)
        {
            return await bookRepo.DeleteBookAsync(id);
        }

        // sort books by price in ascending order 
        public async Task<IEnumerable<BookResponseModel>> GetBooksByPriceAscAsync()
        {
            return await bookRepo.GetBooksByPriceAscAsync();
        }

        // sort books by price in descending order 
        public async Task<IEnumerable<BookResponseModel>> GetBooksByPriceDescAsync()
        {
            return await bookRepo.GetBooksByPriceDescAsync();
        }

        //search books by book name, author name , or recently added books
        public async Task<IEnumerable<BookResponseModel>> SearchBooksAsync(string keyword)
        {
            return await bookRepo.SearchBooksAsync(keyword);
        }
    }
}
