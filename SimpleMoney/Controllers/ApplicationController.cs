using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleMoney.Models;
using WebMatrix.WebData;

namespace SimpleMoney.Controllers
{
     
    public class ApplicationController : Controller
    {
        private EntitiesContext db = new EntitiesContext();

        //
        // GET: /Application/

        public ActionResult Index()
        {
            var applications = db.Applications.Include(a => a.Owner);
            return View(applications.ToList());
        }

        //
        // GET: /Application/Details/5

        public ActionResult Details(int id = 0)
        {
            Application application = db.Applications.Find(id);
            if (application == null)
            {
                return HttpNotFound();
            }
            return View(application);
        }


          //
          // GET: /Application/BusinessDetails/5

          public ActionResult BusinessDetails(int id = 0)
          {

               if (id != 0)
               {
                    Application application = db.Applications.Find(id);

                    if (application != null)
                    {
                         if (application.Owner.Email == User.Identity.Name)
                         {
                              var details = new BusinessDetails();
                              details.ApplicationID = application.ApplicationID;
                              details.ABN = application.ABN;
                              details.ACN = application.ACN;
                              details.LegalName = application.LegalName;
                              

                              return View(details);
                         }
                         else
                         {
                              return RedirectToAction("Index");
                         }
                    }
               }

               return View();
          }

          //
          // POST: /Application/BusinessDetails

          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult BusinessDetails(BusinessDetails details)
          {
               if (ModelState.IsValid)
               {
                    if (details.ApplicationID == 0)
                    {
                         var newApplication = new Application();

                         newApplication.CreateDate = DateTime.Now;
                         newApplication.UserID = WebSecurity.CurrentUserId;
                         
                         newApplication.ABN = details.ABN;
                         newApplication.ACN = details.ACN;
                         newApplication.LegalName = details.LegalName;
                         newApplication.Status = ApplicationStatus.Started;

                         db.Applications.Add(newApplication);

                         db.SaveChanges();                   

                         UserApplication newEntry = new UserApplication();
                         newEntry.AccessType = "owner";
                         newEntry.ApplicationID = newApplication.ApplicationID;
                         newEntry.UserID = WebSecurity.CurrentUserId;

                         db.UserApplications.Add(newEntry);
                         db.SaveChanges();

                         details.ApplicationID = newApplication.ApplicationID;

                    }
                    else
                    {
                         Application existingApplication = db.Applications.Find(details.ApplicationID);

                         existingApplication.ABN = details.ABN;
                         existingApplication.ACN = details.ACN;
                         existingApplication.LegalName = details.LegalName;

                         db.SaveChanges();                   
                    }

                    


                    return RedirectToAction("BusinessFinancials", new { ID = details.ApplicationID });
               }   
            
               return View(details);
          }


        //
        // GET: /Application/BusinessFinancials/5

        public ActionResult BusinessFinancials(int id = 0)
        {
             if (id != 0)
             {
                  Application application = db.Applications.Find(id);
                  if (application != null)
                  {
                       if (application.Owner.Email == User.Identity.Name)
                       {
                            return View(PopulateFinancialsModel(application));
                       }
                  }
             }

              return RedirectToAction("BusinessDetails");
        }

        private static Models.BusinessFinancials PopulateFinancialsModel(Application application)
        {
             var financials = new BusinessFinancials();
             financials.ApplicationID = application.ApplicationID;
             financials.YearlyIncomeBeforeTax = application.YearlyIncomeBeforeTax;
             financials.YearlyDepreciation = application.YearlyDepreciation;
             financials.YearlyInterestEarned = application.YearlyInterestEarned;
             financials.YearlyNetProfit = application.YearlyNetProfit;

             financials.Assets = application.Assets;
             financials.Liabilities = application.Liabilities;
             return financials;
        }

          //
          // POST: /Application/BusinessFinancials

          [HttpPost]
          [ValidateAntiForgeryToken]
          public ActionResult BusinessFinancials(BusinessFinancials  financials, string backButton, string nextButton, string assetButton, string liabilityButton)
          {

               if (backButton != null)
               {
                    return RedirectToAction("BusinessDetails", new { ID = financials.ApplicationID });
               }

               if (assetButton != null)
               {
                    var newAsset = new Asset();

                    newAsset.ApplicationID = financials.ApplicationID;

                    newAsset.Type = financials.AssetType;
                    newAsset.Amount = financials.AssetAmount;
                    newAsset.Details = financials.AssetDetails;

                    db.Assets.Add(newAsset);
                    db.SaveChanges();

                    Application currentApplication = db.Applications.Find(financials.ApplicationID);
                    ModelState.Clear();
                    return View(PopulateFinancialsModel(currentApplication));
               }

               if (liabilityButton != null)
               {
                    var newLiability = new Liability();

                    newLiability.ApplicationID = financials.ApplicationID;

                    newLiability.Type = financials.LiabilityType;
                    newLiability.Amount = financials.LiabilityAmount;
                    newLiability.Details = financials.LiabilityDetails;

                    db.Liabilities.Add(newLiability);
                    db.SaveChanges();

                    Application currentApplication = db.Applications.Find(financials.ApplicationID);

                    ModelState.Clear();
                    return View(PopulateFinancialsModel(currentApplication));
               }


               if (nextButton != null && ModelState.IsValid)
               {
                    Application existingApplication = db.Applications.Find(financials.ApplicationID);

                    existingApplication.YearlyIncomeBeforeTax = financials.YearlyIncomeBeforeTax;
                    existingApplication.YearlyInterestEarned = financials.YearlyInterestEarned;
                    existingApplication.YearlyDepreciation = financials.YearlyDepreciation;
                    existingApplication.YearlyNetProfit = financials.YearlyNetProfit;

                    db.SaveChanges();

                    //TODO Includes some code for sanity checking.

                    return RedirectToAction("LoanRequirements", new { ID = financials.ApplicationID });
               }
               return View(financials);
          }

        //
        // GET: /Application/LoanRequirements/5

        public ActionResult LoanRequirements(int id = 0)
        {
             if (id != 0)
             {
                  Application application = db.Applications.Find(id);
                  if (application != null)
                  {
                       if (application.Owner.Email == User.Identity.Name)
                       {
                            var requirements = new LoanRequirements();
                            requirements.ApplicationID = application.ApplicationID;
                            requirements.LoanAmount = application.LoanAmount;
                            requirements.LoanDurationMonths = application.LoanDurationMonths;
                            requirements.LoanPurpose = application.LoanPurpose;
                            return View(requirements);
                       }
                  }
             }

             return RedirectToAction("BusinessDetails");
        }

        //
        // POST: /Application/LoanRequirements

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoanRequirements(LoanRequirements requirements, string backButton, string nextButton)
        {
             if (backButton != null)
             {
                  return RedirectToAction("BusinessFinancials", new { ID = requirements.ApplicationID });
             }


             if (nextButton != null && ModelState.IsValid)
             {
                  Application existingApplication = db.Applications.Find(requirements.ApplicationID);

                  existingApplication.LoanAmount = requirements.LoanAmount;
                  existingApplication.LoanPurpose = requirements.LoanPurpose;
                  existingApplication.LoanDurationMonths = requirements.LoanDurationMonths;
                  existingApplication.RepaymentTerms = requirements.RepaymentTerms;

                  existingApplication.Status = ApplicationStatus.ReadyForMatches;

                  db.SaveChanges();

                  //TODO Includes some code for sanity checking.

                  return RedirectToAction("ProductSearch", new { ID = requirements.ApplicationID });
             }
             return View(requirements);
        }

        //
        // GET: /Application/ProductSearch/5

        public ActionResult ProductSearch(int id = 0)
        {
             if (id != 0)
             {
                  Application application = db.Applications.Find(id);
                  if (application != null)
                  {
                       if (application.Owner.Email == User.Identity.Name)
                       {
                           
                       }
                  }
             }

             return RedirectToAction("BusinessDetails");
        }

        ////
        //// POST: /Application/LoanRequirements

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ProductSearch(string backButton, string nextButton)
        //{
        //     if (backButton != null)
        //     {
        //          return RedirectToAction("BusinessFinancials", new { ID = requirements.ApplicationID });
        //     }


        //     if (nextButton != null && ModelState.IsValid)
        //     {
        //          Application existingApplication = db.Applications.Find(requirements.ApplicationID);

                  

        //          db.SaveChanges();

        //          //TODO Includes some code for sanity checking.

        //          return RedirectToAction("ProductConfirmation", new { ID = requirements.ApplicationID });
        //     }
        //     return View(requirements);
        //}

       

       

        ////
        //// GET: /Application/Delete/5

        //public ActionResult Delete(int id = 0)
        //{
        //    Application application = db.Applications.Find(id);
        //    if (application == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(application);
        //}

        //
        // POST: /Application/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Application application = db.Applications.Find(id);
            db.Applications.Remove(application);
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