using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Logging;
using DevDefined.OAuth.Storage.Basic;
using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
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

               Session.Add("consumerSession", consumerSession);


               // Wrap the authenticated consumerSession in the repository...
               var repo =  new Repository(consumerSession);

               var org = repo.Organisation;

               ViewBag.Organisation = org.Name;

              

               var latestBalanceSheet = ExtractBalanceSheet(repo);

               var latestProfitAndLossStatement = ExtractProfitAndLossStatement(repo);


               //Session.Add("invoices", invoices.ToList());
               Session.Add("balanceSheet", latestBalanceSheet);
               Session.Add("profitAndLossStatement", latestProfitAndLossStatement);

               return RedirectToAction("Invoices");
          }

          private BalanceSheet ExtractBalanceSheet(Repository repo)
          {
               var latestBalanceSheet = new BalanceSheet();
               latestBalanceSheet.BalanceSheetEntries = new List<BalanceSheetEntry>();

               //Can add a date here in declaration

               var report = new XeroApi.Model.Reporting.BalanceSheetReport();

               var balanceSheetReport = repo.Reports.RunDynamicReport(report);

               var sectionRows = balanceSheetReport.Rows.Where(r => r.RowType == "Section");


               //Sometimes an entry type doesn't don't exist which is why the linqs are in try catch block

               //Assets 
               try
               {

                    var bankEntries = ExtractSectionData(sectionRows.First(r => r.Title == "Bank"), "Bank", true);
                    latestBalanceSheet.BalanceSheetEntries.AddRange(bankEntries);
               }
               catch
               { }

               try
               {
                    var currentAssetEntries = ExtractSectionData(sectionRows.First(r => r.Title == "Current Assets"), "Current Asset", true);
                    latestBalanceSheet.BalanceSheetEntries.AddRange(currentAssetEntries);
               }
               catch { }

               try
               {
                    var fixedAssetEntries = ExtractSectionData(sectionRows.First(r => r.Title == "Fixed Assets"), "Fixed Asset", true);
                    latestBalanceSheet.BalanceSheetEntries.AddRange(fixedAssetEntries);
               }
               catch { }

               //Liabilites
               try
               {
                    var currentLiabilities = ExtractSectionData(sectionRows.First(r => r.Title == "Current Liabilities"), "Current Liability", false);
                    latestBalanceSheet.BalanceSheetEntries.AddRange(currentLiabilities);
               }
               catch
               { }

               try
               {
                    var nonCurrentLiabilities = ExtractSectionData(sectionRows.First(r => r.Title == "Non-Current Liabilities"), "Non-Current Liability", false);
                    latestBalanceSheet.BalanceSheetEntries.AddRange(nonCurrentLiabilities);
               }
               catch { }
               
               

               
               
               
               
               
               

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
               var latestProfitAndLossStatement = new ProfitAndLossStatement();

               latestProfitAndLossStatement.ProfitAndLossStatementEntries = new List<ProfitAndLossStatementEntry>();



               var profitAndLossReport = repo.Reports.RunDynamicReport(report);

               var sectionRows = profitAndLossReport.Rows.Where(r => r.RowType == "Section");


               //Income
               try
               {
                    var operatingIncomeEntries = ExtractPLSectionData(sectionRows.First(r => r.Title == " Income"), ProfitAndLossType.Income);
                    latestProfitAndLossStatement.ProfitAndLossStatementEntries.AddRange(operatingIncomeEntries);
               }
               catch { }

               try
               {
                    var nonOperatingIncome = ExtractPLSectionData(sectionRows.First(r => r.Title == " Non-operating Income"), ProfitAndLossType.OtherIncome); //Leading space in title
                    latestProfitAndLossStatement.ProfitAndLossStatementEntries.AddRange(nonOperatingIncome);
               }
               catch { }

               try
               {
                    //Expenses
                    var costOfSalesEntries = ExtractPLSectionData(sectionRows.First(r => r.Title == " Less Cost Of Sales"), ProfitAndLossType.CostOfSales);
                    latestProfitAndLossStatement.ProfitAndLossStatementEntries.AddRange(costOfSalesEntries);
               }
               catch { }


               try
               {
                    var operatingExpenseEntries = ExtractPLSectionData(sectionRows.First(r => r.Title == " Less Operating Expenses"), ProfitAndLossType.Expense);
                    latestProfitAndLossStatement.ProfitAndLossStatementEntries.AddRange(operatingExpenseEntries);
               }
               catch { }

               return latestProfitAndLossStatement;               
          }

          private List<ProfitAndLossStatementEntry> ExtractPLSectionData(XeroApi.Model.ReportRow sectionRow, ProfitAndLossType type)
          {
               var profitAndLossEntries = new List<ProfitAndLossStatementEntry>();
               try
               {
                    

                    foreach (var row in sectionRow.Rows.Where(r => r.RowType == "Row"))
                    {
                         var newEntry = new ProfitAndLossStatementEntry();

                         newEntry.ProfitAndLossType = type;

                         newEntry.Details = row.Cells[0].Value;
                         newEntry.AccountID = row.Cells[0].Attributes[0].Value;
                         newEntry.Amount = Convert.ToDecimal(row.Cells[1].Value);

                         profitAndLossEntries.Add(newEntry);
                    }

                    
               }
               catch { }
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

          //
          // GET: /Financials/Invoices

          public ActionResult Invoices()
          {
               IOAuthSession consumerSession = (IOAuthSession)Session["consumerSession"];

               // Wrap the authenticated consumerSession in the repository...
               var repo = new Repository(consumerSession);

               var org = repo.Organisation;

               ViewBag.Organisation = org.Name;

               var validAccountsReceivablesInvoices = repo.Invoices.Where(i=>i.Type == "ACCREC" && (i.Status == "AUTHORISED" || i.Status == "PAID")).ToList();

               var unpaidAccRecInvoices = validAccountsReceivablesInvoices.Where(i=>i.Status == "AUTHORISED" && i.DueDate.HasValue);
               
               var paidAccRecInvoices = validAccountsReceivablesInvoices.Where(i=>i.Status == "PAID");

               int averageInvoiceTotal = (int)validAccountsReceivablesInvoices.Average(i => i.Total);

               int unpaidAccRecTotal = (int)unpaidAccRecInvoices.Sum(i => i.Total);

               var averageDaysOverdue = (int)paidAccRecInvoices.Where(i=>i.FullyPaidOnDate.GetValueOrDefault() > i.DueDate.GetValueOrDefault()).Average(i => (i.FullyPaidOnDate.GetValueOrDefault()- i.DueDate.GetValueOrDefault()).Days );

               var averageDaysToPay = (int)paidAccRecInvoices.Average(i => (i.FullyPaidOnDate.GetValueOrDefault() - i.Date.GetValueOrDefault()).Days);

               var invoicesLessThan30DaysOverdue = unpaidAccRecInvoices.Where(i=> DateTime.UtcNow < i.DueDate.Value.AddDays(30));
               var totalLessThan30DaysOverdue = (int) invoicesLessThan30DaysOverdue.Average(i => i.Total);

               var invoices30To60DaysOverdue = unpaidAccRecInvoices.Where(i => DateTime.UtcNow >= i.DueDate.Value.AddDays(30) && DateTime.UtcNow < i.DueDate.Value.AddDays(60));
               var total30To60DaysOverdue = (int) invoices30To60DaysOverdue.Average(i => i.Total);

               var invoices60To90DaysOverdue = unpaidAccRecInvoices.Where(i => DateTime.UtcNow >= i.DueDate.Value.AddDays(60) && DateTime.UtcNow < i.DueDate.Value.AddDays(90));
               var total60To90DaysOverdue = (int)invoices60To90DaysOverdue.Average(i => i.Total);

               var invoicesMoreThan90DaysOverdue = unpaidAccRecInvoices.Where(i => DateTime.UtcNow >= i.DueDate.Value.AddDays(90));
               var totalMoreThan90DaysOverdue = (int?)invoicesMoreThan90DaysOverdue.Average(i => i.Total);


               ViewBag.averageInvoiceTotal = averageInvoiceTotal;
               ViewBag.unpaidAccRecTotal = unpaidAccRecTotal;
               ViewBag.averageDaysOverdue = averageDaysOverdue;
               ViewBag.averageDaysToPay = averageDaysToPay;
               ViewBag.totalLessThan30DaysOverdue = totalLessThan30DaysOverdue;
               ViewBag.total30To60DaysOverdue = total30To60DaysOverdue;
               ViewBag.total60To90DaysOverdue = total60To90DaysOverdue;
               ViewBag.totalMoreThan90DaysOverdue = totalMoreThan90DaysOverdue;

               Highcharts totalOutstandingChart = new Highcharts("chart")
                    .InitChart( new Chart
                    {
                         DefaultSeriesType = ChartTypes.Column
                           
                    }
                    )
                    .SetTitle(new Title { Text = "Total Outstanding Chart" })
                  .SetXAxis(new XAxis
                  {
                       Categories = new[] { "totalLessThan30DaysOverdue", "total30To60DaysOverdue", "total60To90DaysOverdue", "totalMoreThan90DaysOverdue" }
                  })
                  .SetSeries(new Series
                  {
                       Data = new Data(new object[] { ((int)invoicesLessThan30DaysOverdue.Sum(i => i.AmountDue)).ToString(),
                                                      ((int)invoices30To60DaysOverdue.Sum(i=>i.AmountDue)).ToString(),
                                                      ((int)invoices60To90DaysOverdue.Sum(i=>i.AmountDue)).ToString(),
                                                      ((int)invoicesMoreThan90DaysOverdue.Sum(i=>i.AmountDue)).ToString() 
                                                    })
                  });




               ViewBag.totalOutstandingChart = totalOutstandingChart;


               Highcharts averageTotalschart = new Highcharts("chart")
                    .InitChart(new Chart
                    {
                         DefaultSeriesType = ChartTypes.Column
                    }
                    )
                    .SetTitle(new Title { Text = "average Invoice Total Chart" })
                  .SetXAxis(new XAxis
                  {
                       Categories = new[] { "averageInvoiceTotal", "totalLessThan30DaysOverdue", "total30To60DaysOverdue", "total60To90DaysOverdue", "totalMoreThan90DaysOverdue" }
                  })
                  .SetSeries(new Series
                  {
                       Data = new Data(new object[] { averageInvoiceTotal.ToString(), totalLessThan30DaysOverdue, total30To60DaysOverdue, total60To90DaysOverdue, totalMoreThan90DaysOverdue })
                  });




               ViewBag.averageTotalschart = averageTotalschart;



               return View(validAccountsReceivablesInvoices.ToList());
          }

          //
          // GET: /Financials/Invoice

          public ActionResult Invoice(string invoiceID)
          {
               
               IOAuthSession consumerSession = (IOAuthSession)Session["consumerSession"];

               // Wrap the authenticated consumerSession in the repository...
               var repo = new Repository(consumerSession);

               var org = repo.Organisation;

               ViewBag.Organisation = org.Name;

               var invoice = repo.Invoices.Where(i => i.InvoiceID == new Guid(invoiceID)).First();

               return View(invoice);
          }
          

       
    }
}
