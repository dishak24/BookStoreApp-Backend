using RepositoryLayer.Entity;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interfaces
{
    public interface IBooksRepo
    {
        //to get all books
        Task<IEnumerable<Books>> GetAllBooksAsync();

        //get book by id
        public Task<Books> GetBookByIdAsync(int id);

        //update book
        public Task<bool> UpdateBookAsync(int id, BookModel book);

        //delete book
        public Task<bool> DeleteBookAsync(int id);

        // sort books by price in ascending order 
        public Task<IEnumerable<BookResponseModel>> GetBooksByPriceAscAsync();
    }
}
