using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ManagerLayer.Interfaces
{
    public interface IBooksManager
    {

        //to get all books
        public Task<IEnumerable<Books>> GetAllBooksAsync();

        //get book by id
        public Task<Books> GetBookByIdAsync(int id);
    }
}
