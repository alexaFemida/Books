using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Antlr.Runtime.Misc;
using ConscensiaBooks.DataAccess;
using ConscensiaBooks.Models;
using Newtonsoft.Json.Linq;

namespace ConscensiaBooks.Services
{
   public class BookService : IBookService
   {
      const int groupSize = 100;
      public IEnumerable<Book> GetManyBooks(string ISBNs)
      {
         string[] isbns = ISBNs.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
         var booksFromDB = GetBooksFromDB(isbns);
         var isbnsfromDB = booksFromDB.Select(i => i.ISBN);
         var isbnsForServiceCall = (isbns.Except(isbnsfromDB)).ToList();
         var booksFromService = GetBooksFromService(isbnsForServiceCall);
         InsertMany(booksFromService);

         var res = booksFromService.Concat(booksFromDB).ToList();
         return res;
      }

      public PagedData<Book> GetPagedBooks(int currentPage, int pageSize, string ISBNs)
      {
         
            var books = GetManyBooks(ISBNs);

            int totalItemsCount = books.Count();

            return new PagedData<Book>
            {
               TotalItemsCount = totalItemsCount,
               CurrentPage = currentPage,
               PageSize = pageSize,
               Data = books.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
            };
      }
      public List<Book> GetBooksFromDB(string[] isbns)
      {
         using (var dataContext = new BookContext())
         {
            return dataContext.Books.Where(b => isbns.Contains(b.ISBN)).ToList();
         }
      }
      public List<Book> GetBooksFromService(List<string> isbns)
      {
         List<Book> booksFromService = new ListStack<Book>();
         int counOfCalls = (int)Math.Ceiling((double)isbns.Count() / groupSize);
         for (int i = 0; i < counOfCalls; i++)
         {
            var group = isbns.Skip(i * groupSize).Take(groupSize).ToList();
            string isbnsParameter = GetIsbnString(group);
            const string baseUri =
               "http://api.saxo.com/v1/products/products.json?key=08964e27966e4ca99eb0029ac4c4c217&isbn=";

            string requestUri = string.Format("{0}{1}", baseUri, isbnsParameter);
            WebClient webClient = new WebClient();
            webClient.Encoding = Encoding.UTF8;
            dynamic bookDataJs = JObject.Parse(webClient.DownloadString(requestUri));
            booksFromService.AddRange(BookList(bookDataJs));
         }

         return booksFromService;
      }
      static IEnumerable<Book> BookList(dynamic jObject)
      {
         List<Book> books = new List<Book>();
         if (jObject.products != null)
         {
            foreach (var i in jObject.products)
            {
               var book = new Book {Id = i.id, ISBN = i.isbn10, Title = i.title, URL = i.imageurl };
               books.Add(book);
            }
         }
         return books;
      }
      string GetIsbnString(IEnumerable<string> isbnsArray)
      {
         StringBuilder strinbuilder = new StringBuilder();
         foreach (string value in isbnsArray)
         {
            strinbuilder.Append(value);
            strinbuilder.Append(',');
         }
         return strinbuilder.ToString();
      }
      public void InsertMany(List<Book> books)
      {
         if (!books.Any())
            return;

         using (var dataContext = new BookContext())
         {
            books.ForEach(n => dataContext.Books.Add(n));
            int res = dataContext.SaveChanges();
         }
      }
   }
}