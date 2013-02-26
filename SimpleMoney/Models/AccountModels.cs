using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace SimpleMoney.Models
{
     [Table("UserProfile")]
     public class UserProfile
     {
          [Key]
          [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
          public int UserId { get; set; }
          public string Email { get; set; }
          public string MobileNumber { get; set; }

          public string Title { get; set; }
          public string FirstName { get; set; }
          public string LastName { get; set; }
        

          public string FullName
          {
              get
              {
                   return FirstName + " " + LastName;
              }
          }
     }
     

     public class LocalPasswordModel
     {
          [Required]
          [DataType(DataType.Password)]
          [Display(Name = "Current password")]
          public string OldPassword { get; set; }

          [Required]
          [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
          [DataType(DataType.Password)]
          [Display(Name = "New password")]
          public string NewPassword { get; set; }

          [DataType(DataType.Password)]
          [Display(Name = "Confirm new password")]
          [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
          public string ConfirmPassword { get; set; }
     }

     public class LoginModel
     {
          [Required]
          [DataType(DataType.EmailAddress)]
          [Display(Name = "Email")]
          [EmailAddress(ErrorMessage = "This is not a valid email address.")]
          public string Email { get; set; }

          [Required]
          [DataType(DataType.Password)]
          [Display(Name = "Password")]
          public string Password { get; set; }

          [Display(Name = "Remember me?")]
          public bool RememberMe { get; set; }
     }

     public class RegisterModel
     {
          [Required]
          [DataType(DataType.EmailAddress)]
          [Display(Name = "Email")]
          [EmailAddress(ErrorMessage="This is not a valid email address.")]
          public string Email { get; set; }

          [Required]
          [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
          [DataType(DataType.Password)]
          [Display(Name = "Password")]
          public string Password { get; set; }

          [DataType(DataType.Password)]
          [Display(Name = "Confirm password")]
          [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
          public string ConfirmPassword { get; set; }
                    
          [Display(Name = "Mobile Number")]
          public string MobileNumber { get; set; }
     }

    
}
