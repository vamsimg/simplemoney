using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SimpleMoney.Models
{
     [Table("Products")]
     public class Product
     {
          [Key]
          public int ProductID { get; set; }

          public int LenderID { get; set; }

          [ForeignKey("LenderID")]
          public virtual Lender Lender { get; set; }

          public string Title { get; set; }

          public int MinimumAmount { get; set; }
          public int MaximumAmount { get; set; }

          public double InterestRate { get; set; }
          public bool IsVariableRate { get; set; }
          public string Terms { get; set; }
          public FinanceTypes Type { get; set; }

     }

     public enum FinanceTypes
     {          
          LineOfCredit,         
          Overdraft,          
          MortgageLoan,          
          CommercialBill,          
          EquipmentLoan,          
          CreditCard
     }

     public class ProductsInApplication
     {
          public int ApplicationID { get; set; }
          public List<Product> products { get; set; }
     }
}
