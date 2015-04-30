using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using ConscensiaBooks.Services;

namespace ConscensiaBooks.Modules
{
   public class BookModule : Module
   {
      protected override void Load(ContainerBuilder builder)
      {
         builder.RegisterType<BookService>().As<IBookService>();
         base.Load(builder);
      }
   }
}