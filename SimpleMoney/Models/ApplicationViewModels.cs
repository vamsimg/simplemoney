using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SimpleMoney.Models
{
     public class BusinessDetails
     {
          public int ApplicationID { get; set; }
          public string ACN { get; set; }
          public string ABN { get; set; }
          [DisplayName("Legal Name")]
          public string LegalName { get; set; }
     }

     public class BusinessFinancials
     {
          public int ApplicationID { get; set; }

          [DisplayName("Yearly Income(Pre Tax)")]
          public int YearlyIncomeBeforeTax { get; set; }

          [DisplayName("Yearly Depreciation")]
          public int YearlyDepreciation { get; set; }

          [DisplayName("Yearly Interest Earned")]
          public int YearlyInterestEarned { get; set; }

          [DisplayName("Yearly Net Profit")]
          public int YearlyNetProfit { get; set; }

          public virtual List<Asset> Assets { get; set; }
          public virtual List<Liability> Liabilities { get; set; }

          public int AssetAmount { get; set; }
          public string AssetType { get; set; }
          public string AssetDetails { get; set; }

          public int LiabilityAmount { get; set; }
          public string LiabilityType { get; set; }
          public string LiabilityDetails { get; set; }
     }

     public class LoanRequirements
     {
          public int ApplicationID { get; set; }

          [DisplayName("Loan Amount")]
          public int LoanAmount { get; set; }

          [DisplayName("Purpose")]
          public string LoanPurpose { get; set; }

          [DisplayName("Duration")]
          public int LoanDurationMonths { get; set; }

          [DisplayName("Terms")]
          public string RepaymentTerms { get; set; }
     }    
}