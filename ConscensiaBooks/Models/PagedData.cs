using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConscensiaBooks.Models
{
   public class PagedData<T> where T : class
   {
      public IEnumerable<T> Data { get; set; }
      public int CurrentPage { get; set; }
      public int TotalItemsCount { get; set; }
      public int PageSize { get; set; }
   }
}