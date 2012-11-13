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
          Incomplete,
          ReadyForMatches,
          Submitted,
          Cancelled
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
          public virtual Application Application { get; set; }

          [ForeignKey("ProductID")]
          public virtual Product Product { get; set; }

         
          public SubmissionStatus Status { get; set; }
          public DateTime CreateDate { get; set; }
          public DateTime LastModifiedDate { get; set; }     
     }

     public enum SubmissionStatus
     {
          Saved,
          Submitted,          
          Accepted,
          Declined,
          Cancelled
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