using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleMoney.App_Code;
using SimpleMoney.Models;
using WebMatrix.WebData;

namespace SimpleMoney.Controllers
{
     [Authorize]
    public class ApplicationSubmissionController : Controller
    {
        private EntitiesContext db = new EntitiesContext();

        //
        // GET: /ApplicationSubmission/

        public ActionResult Index()
        {
             var currentUser = db.UserProfiles.Find(WebSecurity.CurrentUserId);
            
             var firstPermission = db.LenderEmployees.First(e=>e.UserID == currentUser.UserId);

             if (firstPermission != null)
             {
                  var submissions = db.ApplicationSubmissions.Where(s => s.Product.LenderID == firstPermission.LenderID);
                  return View(submissions.ToList());
             }
             else
             {
                  return RedirectToAction("Index", "Home");
             }            
        }

        //
        // GET: /ApplicationSubmission/Details/5/5

        public ActionResult Details(int ApplicationID = 0, int ProductID = 0)
        {
            ApplicationSubmission applicationSubmission = db.ApplicationSubmissions.Find(ApplicationID, ProductID);

            var permissionFound = db.LenderEmployees.Find(WebSecurity.CurrentUserId, applicationSubmission.Product.LenderID);

            if (applicationSubmission == null)
            {
                 return HttpNotFound();
            }
            else if(permissionFound != null)  
            {
                 return View(applicationSubmission);
            }  
            else
             {
                  return RedirectToAction("Index", "Home");
             }  
        }

          //
          // POST: /ApplicationSubmission/Details

          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult Details(ApplicationSubmission applicationSubmission, string acceptButton, string declineButton )
          {
               ApplicationSubmission submission = db.ApplicationSubmissions.Find(applicationSubmission.ApplicationID, applicationSubmission.ProductID);


               if (acceptButton != null)
               {
                    submission.Status = SubmissionStatus.Accepted;
                    submission.LastModifiedDate = DateTime.Now;

                    EmailHelper.SendLoanApprovedEmail(submission);
               }

               if (declineButton != null)
               {
                    submission.Status = SubmissionStatus.Declined;
                    submission.LastModifiedDate = DateTime.Now;

                    EmailHelper.SendLoanDeclinedEmail(submission);
               }

               db.SaveChanges();

               return RedirectToAction("Index");
          }


        

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}