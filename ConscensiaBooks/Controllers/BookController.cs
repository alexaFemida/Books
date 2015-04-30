using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ConscensiaBooks.Services;

namespace ConscensiaBooks.Controllers
{
   public class BookController : Controller
   {
      readonly IBookService _bookService;

      public BookController(IBookService bookService)
      {
         _bookService = bookService;
      }

      public ActionResult Index()
      {
         string Isbns = "";
         var model = _bookService.GetManyBooks(Isbns);
         return View(model);
      }

      public ActionResult BookList(int currentPage, int pageItemsCount, string isbns)
      {
         var bookList = _bookService.GetPagedBooks(currentPage, pageItemsCount, isbns);

         return PartialView(bookList);
      }

   }
}