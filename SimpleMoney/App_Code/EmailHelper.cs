using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using SimpleMoney.Models;

namespace SimpleMoney.App_Code
{
     public static class EmailHelper
     {
          public const string sender = "vamsi.mg+submit@gmail.com";
          public const string server = "http://localhost:2346/";
          
                   

          public static void SendGenericEmail(string recipient, string subject, string body)
          {
               // create mail message object
               MailMessage mail = new MailMessage();
               mail.From = new MailAddress(sender);		       // put the from address here
               mail.To.Add(new MailAddress(recipient));             // put to address here
               mail.Subject = subject;						  // put subject here			
               mail.Body = body;							  // put body of email here
               SmtpClient client = new SmtpClient();
               try
               {
                    client.Send(mail);
               }
               catch (Exception ex)
               {
                    throw ex;
               }
          }

          public static void SendLoanSubmittedEmail(ApplicationSubmission newSubmission)
          {
               // create mail message object
               MailMessage mail = new MailMessage();
               mail.From = new MailAddress(sender);		       // put the from address here
               mail.To.Add(new MailAddress(newSubmission.Product.Lender.Email));
               mail.Subject = "New submission from a business owner at SimpleMoney";			  // put subject here	

               string serverPath = HttpContext.Current.Server.MapPath("/EmailTemplates/");
               string body = File.ReadAllText(serverPath + "SubmitApplicationEmail.txt");

               body = body.Replace("$FullName", newSubmission.Application.Owner.FullName);

               

               string link = server + "Application/Details/" + newSubmission.ApplicationID.ToString();

               body = body.Replace("$ApplicationLink", link);

               mail.Body = body;



               SmtpClient client = new SmtpClient();
               try
               {
                    client.Send(mail);
               }
               catch (Exception ex)
               {
                    throw ex;
               }
          }


          public static void SendLoanApprovedEmail(ApplicationSubmission newSubmission)
          {
               // create mail message object
               MailMessage mail = new MailMessage();
               mail.From = new MailAddress(sender);		       // put the from address here
               mail.To.Add(new MailAddress(newSubmission.Application.Owner.Email));
               mail.Subject = "Simple Money Loan Application Approved";			  // put subject here	

               string serverPath = HttpContext.Current.Server.MapPath("/EmailTemplates/");
               string body = File.ReadAllText(serverPath + "SubmissionApproved.txt");

               body = body.Replace("$fullName", newSubmission.Application.Owner.FullName);
               body = body.Replace("$product", newSubmission.Product.Title);
               body = body.Replace("$lender", newSubmission.Product.Lender.Name);

               mail.Body = body;



               SmtpClient client = new SmtpClient();
               try
               {
                    client.Send(mail);
               }
               catch (Exception ex)
               {
                    throw ex;
               }
          }

          public static void SendLoanDeclinedEmail(ApplicationSubmission newSubmission)
          {
               // create mail message object
               MailMessage mail = new MailMessage();
               mail.From = new MailAddress(sender);		       // put the from address here
               mail.To.Add(new MailAddress(newSubmission.Application.Owner.Email));
               mail.Subject = "Simple Money Loan Application Declined";			  // put subject here	

               string serverPath = HttpContext.Current.Server.MapPath("/EmailTemplates/");
               string body = File.ReadAllText(serverPath + "SubmissionDeclined.txt");

               body = body.Replace("$fullName", newSubmission.Application.Owner.FullName);
               body = body.Replace("$product", newSubmission.Product.Title);
               body = body.Replace("$lender", newSubmission.Product.Lender.Name);
               body = body.Replace("$declinedMessage", "Insufficient free income to pay loan");

               mail.Body = body;



               SmtpClient client = new SmtpClient();
               try
               {
                    client.Send(mail);
               }
               catch (Exception ex)
               {
                    throw ex;
               }
          }
     }
}