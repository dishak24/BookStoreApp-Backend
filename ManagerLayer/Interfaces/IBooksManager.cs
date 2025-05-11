using RepositoryLayer.Entity;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interfaces
{
    public interface IBooksManager
    {

        //to get all books
        public Task<IEnumerable<BookResponseModel>> GetAllBooksAsync();

        //get book by id
        public Task<BookResponseModel> GetBookByIdAsync(int id);

        //update book
        public Task<bool> UpdateBookAsync(int id, BookModel book);

        //delete book
        public Task<bool> DeleteBookAsync(int id);

        // sort books by price in ascending order 
        public Task<IEnumerable<BookResponseModel>> GetBooksByPriceAscAsync();

        // sort books by price in descending order 
        public Task<IEnumerable<BookResponseModel>> GetBooksByPriceDescAsync();

        //search books by book name, author name , or recently added books
        public Task<IEnumerable<BookResponseModel>> SearchBooksAsync(string keyword);
    }
}
