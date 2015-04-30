using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using ConscensiaBooks.Modules;

namespace ConscensiaBooks
{
   public class MvcApplication : System.Web.HttpApplication
   {
      protected void Application_Start()
      {
         var builder = new ContainerBuilder();
         builder.RegisterControllers(Assembly.GetExecutingAssembly());
         builder.RegisterModule<BookModule>();
         var container = builder.Build();
         DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

         AreaRegistration.RegisterAllAreas();
         FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
         RouteConfig.RegisterRoutes(RouteTable.Routes);
         BundleConfig.RegisterBundles(BundleTable.Bundles);
      }
   }
}
