using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Logging;
using DevDefined.OAuth.Storage.Basic;
using SimpleMoney.Models;
using XeroApi;
using XeroApi.OAuth;


namespace SimpleMoney.Controllers
{
    public class FinancialsController : Controller
    {
         private const string UserAgent = "Simple Money Experimental";
         private const string ConsumerKey = "IO4E6CNRAHWQ8SVZQKLU5FEDBPBESZ";
         private const string ConsumerSecret = "SVJKYDJ1GCABWGONQIKHEKWIKNSDFN";

        //
        // GET: /Financials/Index

        public ActionResult Index()
        {
             IOAuthSession consumerSession = new XeroApiPublicSession(UserAgent, ConsumerKey, ConsumerSecret, new InMemoryTokenRepository());

             Session.Add("consumerSession", consumerSession);

             consumerSession.MessageLogger = new DebugMessageLogger();

             // 1. Get a request token
             IToken requestToken = consumerSession.GetRequestToken();


             // 2. Get the user to log into Xero using the request token in the querystring
             string authorisationUrl = consumerSession.GetUserAuthorizationUrl();
             ViewBag.XeroLink = authorisationUrl;
             return View();
        }

          //
          // POST: /Financials/Index

          [HttpPost]
          [AllowAnonymous]
          [ValidateAntiForgeryToken]
          public ActionResult Index(string verificationCode)
          {
               // 4. Use the request token and verification code to get an access token
               AccessToken accessToken;

               IOAuthSession consumerSession = (IOAuthSession)Session["consumerSession"];

               try
               {
                    accessToken = consumerSession.ExchangeRequestTokenForAccessToken(verificationCode.Trim());
               }
               catch (OAuthException ex)
               {
                    Console.WriteLine("An OAuthException was caught:");
                    Console.WriteLine(ex.Report);
                    return null;
               }

             


               // Wrap the authenticated consumerSession in the repository...
               var repo =  new Repository(consumerSession);

               var org = repo.Organisation;

               ViewBag.Organisation = org.Name;





               var latestBalanceSheet = ExtractBalanceSheet(repo);

               var latestProfitAndLossStatement = ExtractProfitAndLossStatement(repo);


               Session.Add("balanceSheet", latestBalanceSheet);
               Session.Add("profitAndLossStatement", latestProfitAndLossStatement);

               return RedirectToAction("ProfitAndLossStatements");
          }

          private BalanceSheet ExtractBalanceSheet(Repository repo)
          {
               //Can add a date here in declaration

               var report = new XeroApi.Model.Reporting.BalanceSheetReport();

               var balanceSheetReport = repo.Reports.RunDynamicReport(report);

               var sectionRows = balanceSheetReport.Rows.Where(r => r.RowType == "Section");


               //Assets
               var bankEntries = ExtractSectionData(sectionRows.First(r => r.Title == "Bank"), "Bank", true);
               var currentAssetEntries = ExtractSectionData(sectionRows.First(r => r.Title == "Current Assets"), "Current Asset", true);
               var fixedAssetEntries = ExtractSectionData(sectionRows.First(r => r.Title == "Fixed Assets"), "Fixed Asset", true);

               //Liabilites
               var currentLiabilities = ExtractSectionData(sectionRows.First(r => r.Title == "Current Liabilities"), "Current Liability", false);
               var nonCurrentLiabilities = ExtractSectionData(sectionRows.First(r => r.Title == "Non-Current Liabilities"), "Non-Current Liability", false);

               var latestBalanceSheet = new BalanceSheet();

               latestBalanceSheet.BalanceSheetEntries = new List<BalanceSheetEntry>();
               latestBalanceSheet.BalanceSheetEntries.AddRange(bankEntries);
               latestBalanceSheet.BalanceSheetEntries.AddRange(currentAssetEntries);
               latestBalanceSheet.BalanceSheetEntries.AddRange(fixedAssetEntries);
               latestBalanceSheet.BalanceSheetEntries.AddRange(currentLiabilities);
               latestBalanceSheet.BalanceSheetEntries.AddRange(nonCurrentLiabilities);

               return latestBalanceSheet;
          }

          private List<BalanceSheetEntry> ExtractSectionData(XeroApi.Model.ReportRow sectionRow, string sectionTitle, bool isAsset)
          {
               var balanceSheetEntries = new List<BalanceSheetEntry>();

               foreach (var row in sectionRow.Rows.Where(r => r.RowType == "Row"))
               {
                    var balanceSheetEntry = new BalanceSheetEntry();

                    balanceSheetEntry.isAsset = isAsset;
                    balanceSheetEntry.Type = sectionTitle;

                    balanceSheetEntry.Details = row.Cells[0].Value;
                    balanceSheetEntry.AccountID = row.Cells[0].Attributes[0].Value;
                    balanceSheetEntry.Amount = Convert.ToDecimal(row.Cells[1].Value);

                    balanceSheetEntries.Add(balanceSheetEntry);
               }

               return balanceSheetEntries;
          }
          
          
          /// <summary>
          /// Beware some of the titles have leading spaces so if Xero fixes them the method will break. 
          /// </summary>
          /// <param name="repo"></param>
          /// <returns></returns>
          private ProfitAndLossStatement ExtractProfitAndLossStatement(Repository repo)
          {
               //Can add a date here in declaration

               var report = new XeroApi.Model.Reporting.ProfitAndLossReport();

               var profitAndLossReport = repo.Reports.RunDynamicReport(report);

               var sectionRows = profitAndLossReport.Rows.Where(r => r.RowType == "Section");


               //Income
               var operatingIncomeEntries = ExtractPLSectionData(sectionRows.First(r => r.Title == " Income"), ProfitAndLossType.Income);
               var nonOperatingIncome = ExtractPLSectionData(sectionRows.First(r => r.Title == " Non-operating Income"), ProfitAndLossType.OtherIncome); //Leading space in title
               
               //Expenses
               var costOfSalesEntries = ExtractPLSectionData(sectionRows.First(r => r.Title == " Less Cost Of Sales"), ProfitAndLossType.CostOfSales);
               var operatingExpenseEntries = ExtractPLSectionData(sectionRows.First(r => r.Title == " Less Operating Expenses"), ProfitAndLossType.Expense);

               var latestProfitAndLossStatement = new ProfitAndLossStatement();

               latestProfitAndLossStatement.ProfitAndLossStatementEntries = new List<ProfitAndLossStatementEntry>();

               latestProfitAndLossStatement.ProfitAndLossStatementEntries.AddRange(operatingIncomeEntries);
               latestProfitAndLossStatement.ProfitAndLossStatementEntries.AddRange(nonOperatingIncome);
               latestProfitAndLossStatement.ProfitAndLossStatementEntries.AddRange(costOfSalesEntries);
               latestProfitAndLossStatement.ProfitAndLossStatementEntries.AddRange(operatingExpenseEntries);

               return latestProfitAndLossStatement;               
          }

          private List<ProfitAndLossStatementEntry> ExtractPLSectionData(XeroApi.Model.ReportRow sectionRow, ProfitAndLossType type)
          {
               var profitAndLossEntries = new List<ProfitAndLossStatementEntry>();

               foreach (var row in sectionRow.Rows.Where(r => r.RowType == "Row"))
               {
                    var newEntry = new ProfitAndLossStatementEntry();

                    newEntry.ProfitAndLossType = type;

                    newEntry.Details = row.Cells[0].Value;
                    newEntry.AccountID = row.Cells[0].Attributes[0].Value;
                    newEntry.Amount = Convert.ToDecimal(row.Cells[1].Value);

                    profitAndLossEntries.Add(newEntry);
               }

               return profitAndLossEntries;
          }

          //
          // GET: /Financials/BalanceSheets

          public ActionResult BalanceSheets()
          {    
               var balanceSheet = (BalanceSheet)Session["balanceSheet"];

               return View(balanceSheet);
          }

          //
          // GET: /Financials/ProfitAndLossStatements

          public ActionResult ProfitAndLossStatements()
          {
               var profitAndLossStatement = (ProfitAndLossStatement)Session["profitAndLossStatement"];

               return View(profitAndLossStatement);
          }

          
          

       
    }
}
