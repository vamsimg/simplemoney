using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SimpleMoney.Models
{
     [Table("Lenders")]
     public class Lender
     {
          [Key]
          public int LenderID { get; set; }
          public string Name { get; set; }
          public string ACN { get; set; }
          public string Email { get; set; }

          public virtual List<Product> Products { get; set; }
     }     
}