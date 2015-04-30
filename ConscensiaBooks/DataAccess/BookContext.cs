using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ConscensiaBooks.Models;

namespace ConscensiaBooks.DataAccess
{
   public class BookContext : DbContext
   {
      public BookContext()
         : base("name=BooksConnectionString")
      {
         Database.SetInitializer<BookContext>(null);
      }

      public virtual DbSet<Book> Books { get; set; }
   }
}