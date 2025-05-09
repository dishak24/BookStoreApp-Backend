using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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

        //to get book by id
        public async Task<Books> GetBookByIdAsync(int id)
        {
            return await context.Books.FindAsync(id);
        }

        //update book
        public async Task<bool> UpdateBookAsync(int id, BookModel updatedBook)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null)
            {
                return false;
            }
                
            book.BookName = updatedBook.BookName;
            book.Author = updatedBook.Author;
            book.Description = updatedBook.Description;
            book.Price = updatedBook.Price;
            book.DiscountPrice = updatedBook.DiscountPrice;
            book.Quantity = updatedBook.Quantity;
            book.BookImage = updatedBook.BookImage;
            book.UpdatedAt = DateTime.Now;

            await context.SaveChangesAsync();
            return true;
        }


        //delete book
        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null)
            {
                return false;
            }

            context.Books.Remove(book);
            await context.SaveChangesAsync();
            return true;
        }

        // sort books by price in ascending order 
        public async Task<IEnumerable<BookResponseModel>> GetBooksByPriceAscAsync()
        {
            return await context.Books
               .OrderBy(b => b.Price)
               .Select(b => new BookResponseModel
               {
                   BookName = b.BookName,
                   Author = b.Author,
                   Description = b.Description,
                   Price = b.Price,
                   DiscountPrice = b.DiscountPrice,
                   Quantity = b.Quantity,
                   BookImage = b.BookImage
               })
               .ToListAsync();
        }

        
    }
}
