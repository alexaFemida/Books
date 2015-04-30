using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ConscensiaBooks.Helpers
{
   public static class Pager
   {
      static readonly List<string> disabledItemsCssClasses = new List<string>() { "disabled", "active" };
      static TagBuilder ElementBuilder(string linkText, int page, string cssClass)
      {
         TagBuilder li = new TagBuilder("li");
         var link = new TagBuilder("a");
         link.SetInnerText(linkText);

         if (!string.IsNullOrEmpty(cssClass))
            li.AddCssClass(cssClass);
         li.MergeAttribute("value", page.ToString());
         if (string.IsNullOrEmpty(cssClass) || !disabledItemsCssClasses.Contains(cssClass.ToLower()))
         {
            link.MergeAttribute("href", "#");
         }
         li.InnerHtml += link.ToString();
         return li;
      }

      static TagBuilder ElementBuilder(int page, string cssClass)
      {
         return ElementBuilder(page.ToString(), page, cssClass);
      }

      static TagBuilder DotsBuilder()
      {
         return ElementBuilder("...", 0, "disabled");
      }

      public static MvcHtmlString PagerListing(this HtmlHelper helper, int currentPage, int pageSize, int totalItemCount)
      {
         int biggerGroupCount = 4;
         int smallerGroupCount = 3;

         var pageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);

         if (pageCount == 0)
            return MvcHtmlString.Create("");

         currentPage = Math.Max(currentPage, 1);
         currentPage = Math.Min(currentPage, pageCount);

         var container = new TagBuilder("div");
         container.AddCssClass("pagination");

         var lastGroupNumber = currentPage;
         while ((lastGroupNumber % pageCount != 0)) lastGroupNumber++;

         var groupEnd = Math.Min(lastGroupNumber, pageCount);
         var groupStart = lastGroupNumber - (pageCount - 1);

         TagBuilder ul = new TagBuilder("ul");
         var prev = ElementBuilder("Prev", Math.Max(currentPage - 1, 1), currentPage > 1 ? "page-number" : "disabled");
         ul.InnerHtml += prev.ToString();

         if (pageCount < 10)
         {
            for (var i = 1; i <= pageCount; i++)
            {
               var li = ElementBuilder(i, i == currentPage ? "active" : "page-number");
               ul.InnerHtml += li.ToString();
            }
         }
         else
         {
            if (currentPage == (groupStart + biggerGroupCount - 1))
            {
               for (var i = groupStart; i <= (groupStart + biggerGroupCount); i++)
               {
                  var li = ElementBuilder(i, i == currentPage ? "active" : "page-number");
                  ul.InnerHtml += li.ToString();
               }

               var liDots = DotsBuilder();
               ul.InnerHtml += liDots.ToString();

               for (var i = groupEnd - smallerGroupCount; i < groupEnd; i++)
               {
                  var li = ElementBuilder(i, i == currentPage ? "active" : "page-number");
                  ul.InnerHtml += li.ToString();
               }
            }
            else
               if (currentPage == (groupEnd - biggerGroupCount + 1))
               {
                  for (var i = groupStart; i <= (groupStart + smallerGroupCount - 1); i++)
                  {
                     var li = ElementBuilder(i, i == currentPage ? "active" : "page-number");
                     ul.InnerHtml += li.ToString();
                  }

                  var liDots = DotsBuilder();
                  ul.InnerHtml += liDots.ToString();

                  for (var i = groupEnd - biggerGroupCount; i <= groupEnd; i++)
                  {
                     var li = ElementBuilder(i, i == currentPage ? "active" : "page-number");
                     ul.InnerHtml += li.ToString();
                  }
               }
               else
                  if ((currentPage < (groupStart + biggerGroupCount - 1) ||
                      (currentPage > (groupEnd - biggerGroupCount + 1))))
                  {
                     for (var i = groupStart; i < (groupStart + biggerGroupCount); i++)
                     {
                        var li = ElementBuilder(i, i == currentPage ? "active" : "page-number");
                        ul.InnerHtml += li.ToString();
                     }

                     if ((groupEnd - 2 * biggerGroupCount) > 1)
                     {
                        var liDots = DotsBuilder();
                        ul.InnerHtml += liDots.ToString();
                     }

                     for (var i = groupEnd - biggerGroupCount + 1; i <= groupEnd; i++)
                     {
                        var li = ElementBuilder(i, i == currentPage ? "active" : "page-number");
                        ul.InnerHtml += li.ToString();
                     }
                  }
                  else
                     if ((currentPage > groupStart + biggerGroupCount - 1) &&
                        (currentPage < groupEnd - biggerGroupCount + 1))
                     {
                        for (var i = groupStart; i <= groupStart + smallerGroupCount; i++)
                        {
                           if (i == groupStart + smallerGroupCount && currentPage != i + 2)
                              continue;
                           var li = ElementBuilder(i, i == currentPage ? "active" : "page-number");
                           ul.InnerHtml += li.ToString();
                        }

                        if (groupStart + smallerGroupCount < currentPage - 2)
                        {
                           var liDots = DotsBuilder();
                           ul.InnerHtml += liDots.ToString();
                        }

                        for (var i = currentPage - 1; i <= currentPage + 1; i++)
                        {
                           var li = ElementBuilder(i, i == currentPage ? "active" : "page-number");
                           ul.InnerHtml += li.ToString();
                        }

                        if (groupEnd - smallerGroupCount > currentPage + 2)
                        {
                           var liDots = DotsBuilder();
                           ul.InnerHtml += liDots.ToString();
                        }

                        for (var i = groupEnd - smallerGroupCount; i <= groupEnd; i++)
                        {
                           if (i == groupEnd - smallerGroupCount && currentPage != i - 2)
                              continue;
                           var li = ElementBuilder(i, i == currentPage ? "active" : "page-number");
                           ul.InnerHtml += li.ToString();
                        }
                     }
         }
         var next = ElementBuilder("Next", Math.Min(currentPage + 1, pageCount),
            currentPage < pageCount ? "page-number" : "disabled");
         ul.InnerHtml += next.ToString();

         container.InnerHtml += ul.ToString();
         return MvcHtmlString.Create(container.ToString());
      }
   }
}
