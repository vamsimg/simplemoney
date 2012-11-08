using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SimpleMoney.Models
{
     [Table("Applications")]
     public class Application
     {
          [Key]
          public int ApplicationID { get; set; }

          public int UserID { get; set; }

          [ForeignKey("UserID")]
          public virtual UserProfile Owner { get; set; }        
          public string ACN { get; set; }
          public string ABN { get; set; }
          public string LegalName { get; set; }
          public DateTime CreateDate { get; set; }

          
          public int YearlyIncomeBeforeTax { get; set; }          
          public int YearlyDepreciation { get; set; }          
          public int YearlyInterestEarned { get; set; }          
          public int YearlyNetProfit { get; set; }  
          
          public int LoanAmount { get; set; }          
          public string LoanPurpose { get; set; }          
          public int LoanDurationMonths { get; set; }          
          public string RepaymentTerms { get; set; }

          public ApplicationStatus Status { get; set; }

          public virtual List<Asset> Assets { get; set; }
          public virtual List<Liability> Liabilities { get; set; }

          public virtual List<ApplicationBacker> ApplicationBackers { get; set; }
          public virtual List<UserApplication> UserApplications { get; set; }
          public virtual List<ApplicationSubmission> ApplicationSubmissions { get; set; }

     }

     public enum ApplicationStatus
     {
          Started,
          ReadyForMatches,
          Submitted,
          Cancelled
     }    

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

     [Table("ApplicationBackers")]
     public class ApplicationBacker
     {
          [Key]
          public int ApplicationBackerID { get; set; }
                    
          public int ApplicationID { get; set; }

          [ForeignKey("ApplicationID")]
          public Application Application { get; set; }

          public string Title { get; set; }
          public string FirstName { get; set; }
          public string LastName { get; set; }
          public string RelationshipToBusiness { get; set; }
          public string Email { get; set; }
          public string MobileNumber { get; set; }
          
     }

     [Table("UserApplications")]
     public class UserApplication
     {
          [Key, Column(Order = 0)]          
          public int UserID { get; set; }

          [Key, Column(Order = 1)]          
          public int ApplicationID { get; set; }

          [ForeignKey("ApplicationID")]
          public Application Application { get; set; }
          
          [ForeignKey("UserID")]
          public UserProfile User { get; set; }
                   
          public string AccessType { get; set; }
     }

     [Table("ApplicationSubmissions")]
     public class ApplicationSubmission
     {
          [Key, Column(Order = 0)]
          public int ApplicationID { get; set; }

          [Key, Column(Order = 1)]
          
          public int ProductID { get; set; }

          [ForeignKey("ApplicationID")]
          public Application Application { get; set; }

          [ForeignKey("ProductID")]
          public Product Product { get; set; }

          //TODO does this need to be split out into an enumeration.
          public SubmissionStatus Status { get; set; }
          public DateTime CreateDate { get; set; }     
     }

     public enum SubmissionStatus
     {
          Submitted,
          Accepted,
          AwaitingDecision,
          Declined
     }

     [Table("Assets")]
     public class Asset
     {
          [Key]
          public int AssetID { get; set; }
                    
          public int ApplicationID { get; set; }

          [ForeignKey("ApplicationID")]
          public Application Application { get; set; }

          public int Amount { get; set; }
          public string Type { get; set; }
          public string Details { get; set; }
     }

     [Table("Liabilities")]
     public class Liability
     {
          [Key]
          public int LiabilityID { get; set; }

          public int ApplicationID { get; set; }

          [ForeignKey("ApplicationID")]
          public Application Application { get; set; }
          
          public int Amount { get; set; }
          public string Type { get; set; }
          public string Details { get; set; }
     }
}