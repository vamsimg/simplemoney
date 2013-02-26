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
          public string Comments { get; set; }
          public string Purpose { get; set; }

          public int MinimumAmount { get; set; }
          public int MaximumAmount { get; set; }
          public string LoanAmountNotes { get; set; }
          
          public int LVR { get; set; }
          public string LVRNotes { get; set; }

          public int MinimumDuration { get; set; }
          public int MaximumDuration { get; set; }
          public string MaximumDurationNotes { get; set; }

          public double InterestRate { get; set; }
          public RateType? RateTypes  { get; set; }

          public SecurityType? SecurityTypes  { get; set; }
          public string SecurityNotes { get; set; }

          public bool ProgressDrawingsAllowed { get; set; }

          public RepaymentType? RepaymentTypes { get; set; }
          public FrequencyType? RepaymentFrequencies { get; set; }
          public string RepaymentNotes { get; set; }

          public FrequencyType? InterestCalculationFrequency { get; set; }

          //Fees

          public double EstablishmentFeePercentage { get; set; }
          public int EstablishmentFeeMinimum { get; set; }
          public string EstablishmentFeeNotes { get; set; }

          public int MonthlyAccountKeepingFee { get; set; }
          public int TerminationFee { get; set; }

          public bool EarlyRepaymentsAllowed { get; set; }
          public string EarlyRepaymentNotes { get; set; }

          public string ProductSpecificInfo { get; set; }

                  
          public ProductCategory ProductCategory { get; set; }
     }
      
     [Flags]
     public enum RateType: byte
     {
          Variable = 1,
          Fixed = 2
     }

     [Flags]
     public enum SecurityType: byte
     {
          ResidentialProperty = 1,
          CommercialProperty = 2,
          None = 4 ,
          Plant = 8,
          Equipment = 16,
          Vehicle = 32,
          Cash = 64
     }
     
     [Flags]
     public enum RepaymentType : byte
     {          InterestInArrears = 1,
          InterestInAdvance = 2,
          PrincipalPlusInterest = 4,
          None = 8
     }

     [Flags]
     public enum FrequencyType : byte
     {
          Daily = 1,
          Weekly = 2,
          Fortnightly = 4,
          Monthly = 8,
          Quarterly = 16,
          HalfYearly = 32,
          Yearly = 64         
     }

     public enum ProductCategory
     {
          Overdraft,
          LineOfCredit,
          TermLoan,
          ReceivablesFinance,
          CommercialBill,
          LeasingFinance        
     }

     public class ProductsInApplication
     {
          public int ApplicationID { get; set; }
          public List<Product> products { get; set; }
     }
     
}
