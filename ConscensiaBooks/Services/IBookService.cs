using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConscensiaBooks.Models;
using ConscensiaBooks.Models;

namespace ConscensiaBooks.Services
{
   public interface IBookService
   {
      IEnumerable<Book> GetManyBooks(string ISBNs);
      PagedData<Book> GetPagedBooks(int currentPage, int pageSize, string ISBNs);
      void InsertMany(List<Book> books);
   }
}
