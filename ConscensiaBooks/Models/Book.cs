using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using ConscensiaBooks.Helpers;

namespace ConscensiaBooks.Models
{
   [Table("Books")]
   public class Book
   {
      [Key]
      public string Id { get; set; }
      [Required]
      public string ISBN { get; set; }
      [Required]
      public string Title { get; set; }
      public string URL { get; set; }
   }
}