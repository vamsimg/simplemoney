namespace SimpleMoney.Migrations
{
     using System;
     using System.Collections.Generic;
     using System.Data.Entity;
     using System.Data.Entity.Migrations;
     using System.Linq;
     using System.Web.Security;
     using SimpleMoney.Models;
     using WebMatrix.WebData;

     internal sealed class Configuration : DbMigrationsConfiguration<SimpleMoney.Models.EntitiesContext>
     {
          public Configuration()
          {
               AutomaticMigrationsEnabled = true;
               
          }

          protected override void Seed(SimpleMoney.Models.EntitiesContext context)
          {
               //  This method will be called after migrating to the latest version.

               //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
               //  to avoid creating duplicate seed data. E.g.
               //
               //    context.People.AddOrUpdate(
               //      p => p.FullName,
               //      new Person { FullName = "Andrew Peters" },
               //      new Person { FullName = "Brice Lambson" },
               //      new Person { FullName = "Rowan Miller" }
               //    );
               //

               WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "UserId", "Email", autoCreateTables: true);


               if (!Roles.RoleExists("admin"))
               {
                    Roles.CreateRole("admin");
               }

               if (!Roles.RoleExists("applicant"))
               {
                    Roles.CreateRole("applicant");
               }

               if (!Roles.RoleExists("accountant"))
               {
                    Roles.CreateRole("accountant");
               }

               if (!Roles.RoleExists("lender"))
               {
                    Roles.CreateRole("lender");
               } 

               if(!WebSecurity.UserExists("vamsi.mg@gmail.com"))
               {
                    WebSecurity.CreateUserAndAccount("vamsi.mg@gmail.com", "helloworld", new { MobileNumber = "0432100092" });
               }

               if (!Roles.GetRolesForUser("vamsi.mg@gmail.com").Contains("admin"))
               {                    
                    Roles.AddUsersToRoles(new[] {"vamsi.mg@gmail.com"}, new[] {"admin"});
               }

               if (!WebSecurity.UserExists("vamsi.mg+lender@gmail.com"))
               {
                    WebSecurity.CreateUserAndAccount("vamsi.mg+lender@gmail.com", "helloworld", new { MobileNumber = "0432100092" });
               }

               if (!Roles.GetRolesForUser("vamsi.mg+lender@gmail.com").Contains("lender"))
               {
                    Roles.AddUsersToRoles(new[] { "vamsi.mg+lender@gmail.com" }, new[] { "lender" });
               }

               int lenderUserID = WebSecurity.GetUserId("vamsi.mg+lender@gmail.com");
               
               var lenders = new List<string>();

               lenders.Add("Commonwealth Bank");
               lenders.Add("National Australia Bank");
               lenders.Add("Westpac");
               lenders.Add("St George");
               lenders.Add("ANZ");
               lenders.Add("Suncorp");
               lenders.Add("Bankwest");
               lenders.Add("Westpac");
               lenders.Add("Adelaide Bank");
               



               foreach(var lender in lenders)
               {
                    if(context.Lenders.Where(l=>l.Name == lender).Count() == 0)
                    {
                         var newLender = new Lender();
                         newLender.Name = lender;
                         newLender.ACN = "1001";                    
                         context.Lenders.Add(newLender);
                    }
               }

               context.SaveChanges();
               
               var currentLenders = context.Lenders.ToList();


               #region Commbank

               var commbank = context.Lenders.First(l => l.Name == "Commonwealth Bank");


               var commbankOverdraft = new Product
               {
                    LenderID = commbank.LenderID,
                    Title = "Overdraft",
                    RateTypes = RateType.Variable,
                    ProductCategory = ProductCategory.Overdraft,
                    SecurityTypes  = SecurityType.None,
               };

               var commbankTermLoan = new Product
               {
                    LenderID       = commbank.LenderID,
                    Title          = "Better Business Loan",
                    MinimumAmount  = 50000,
                    LoanAmountNotes = "Minimum Loan Amount with Residential Security is $100000",
                    SecurityTypes  = SecurityType.ResidentialProperty,

                    RateTypes      = RateType.Variable | RateType.Fixed ,

                    ProductCategory = ProductCategory.TermLoan
               };

               var commbankBill = new Product
               {
                    LenderID       = commbank.LenderID,
                    Title          = "Commercial Bill",
                    MinimumAmount  = 500000,                    
                    RateTypes      = RateType.Fixed ,

                    ProductCategory = ProductCategory.CommercialBill
               };

               var commbankLineOfCredit = new Product
               {
                    LenderID = commbank.LenderID,
                    Title = "Line of Credit",
                    MinimumAmount = 5000,                    
                    RateTypes = RateType.Variable | RateType.Fixed,
                    SecurityTypes  = SecurityType.None,
                    ProductCategory = ProductCategory.LineOfCredit
               };

               context.Products.Add(commbankOverdraft);
               context.Products.Add(commbankTermLoan);
               context.Products.Add(commbankBill);
               context.Products.Add(commbankLineOfCredit);

               context.SaveChanges();
               

               #endregion

               #region Bankwest

               var bankwest = context.Lenders.First(l => l.Name == "Bankwest");


               var bankwestOverdraft = new Product
               {
                    LenderID = bankwest.LenderID,
                    Title = "Business Overdrafts",
                    MinimumAmount = 20000,

                    RateTypes = RateType.Variable,
                    SecurityTypes = SecurityType.ResidentialProperty | SecurityType.CommercialProperty ,

                    EstablishmentFeePercentage = 0.5,
                    EstablishmentFeeMinimum = 700,

                    ProductCategory = ProductCategory.Overdraft
               };


               var bankwestLineOfCredit = new Product
               {
                    LenderID = bankwest.LenderID,
                    Title = "Business Equity Line",
                    Comments = @"Fully functioning line of credit
                                  for the purpose of larger 
                                  purchases or investments",

                    
                    MinimumAmount = 50000,
                    RateTypes = RateType.Variable,
                    SecurityTypes = SecurityType.ResidentialProperty | SecurityType.CommercialProperty,

                    EstablishmentFeePercentage = 0.5,
                    EstablishmentFeeMinimum = 700,
                    MonthlyAccountKeepingFee = 33,

                    ProductCategory = ProductCategory.LineOfCredit

               };

               var bankwestTermLoan1 = new Product
               {
                    LenderID = bankwest.LenderID,
                    Title = "Business Low Rate Loan",
                    MinimumAmount = 20000,

                    MinimumDuration = 1,
                    MaximumDuration = 15,
                    MaximumDurationNotes = "Principal & Interest  15 years (Commercial security) 30 years (residential security), Interest only 5 years", 

                    RateTypes = RateType.Variable,
                    SecurityTypes = SecurityType.ResidentialProperty | SecurityType.CommercialProperty,

                    RepaymentFrequencies = FrequencyType.Monthly | FrequencyType.Quarterly | FrequencyType.HalfYearly | FrequencyType.Yearly ,
                    RepaymentTypes = RepaymentType.PrincipalPlusInterest | RepaymentType.InterestInArrears,
                    
                    ProgressDrawingsAllowed = true,
                    EarlyRepaymentsAllowed = true,
                    
                    EstablishmentFeePercentage = 0.5,
                    EstablishmentFeeMinimum = 700,
                    MonthlyAccountKeepingFee = 20,

                    ProductCategory = ProductCategory.TermLoan
               };

               var bankwestTermLoan2 = new Product
               {
                    LenderID = bankwest.LenderID,
                    Title = "Business Fixed Rate Loan",
                    MinimumAmount = 50000,

                    MinimumDuration = 1,
                    MaximumDuration = 15,

                    RateTypes = RateType.Fixed,
                    SecurityTypes = SecurityType.ResidentialProperty | SecurityType.CommercialProperty,

                    RepaymentFrequencies = FrequencyType.Monthly | FrequencyType.Quarterly | FrequencyType.HalfYearly | FrequencyType.Yearly,
                    RepaymentTypes = RepaymentType.PrincipalPlusInterest | RepaymentType.InterestInArrears,


                    EstablishmentFeePercentage = 0.5,
                    EstablishmentFeeMinimum = 700,
                    MonthlyAccountKeepingFee = 20,

                    ProductCategory = ProductCategory.TermLoan
               };

               var bankwestTermLoan3 = new Product
               {
                    LenderID = bankwest.LenderID,
                    Title = "Business Fee Saver Loan",
                    MinimumAmount = 20000,

                    MinimumDuration = 1,
                    MaximumDuration = 15,
                    MaximumDurationNotes = "Principal & Interest  15 years (Commercial security) 30 years (residential security), Interest only 5 years",

                    RateTypes = RateType.Variable,
                    SecurityTypes = SecurityType.ResidentialProperty | SecurityType.CommercialProperty,

                    RepaymentFrequencies = FrequencyType.Monthly | FrequencyType.Quarterly | FrequencyType.HalfYearly | FrequencyType.Yearly,
                    RepaymentTypes = RepaymentType.PrincipalPlusInterest | RepaymentType.InterestInArrears,

                    EarlyRepaymentsAllowed = true,
                    

                    EstablishmentFeePercentage = 0,
                    EstablishmentFeeMinimum = 0,
                    MonthlyAccountKeepingFee = 0,

                    ProductCategory = ProductCategory.TermLoan
               };

               context.Products.Add(bankwestOverdraft);
               context.Products.Add(bankwestLineOfCredit);
               context.Products.Add(bankwestTermLoan1);
               context.Products.Add(bankwestTermLoan2);
               context.Products.Add(bankwestTermLoan3);
               

               context.SaveChanges();

               #endregion


               #region Suncorp

               var suncorp = context.Lenders.First(l => l.Name == "Suncorp");


               var suncorpOverdraft = new Product
               {
                    LenderID = suncorp.LenderID,
                    Title = "Business Overdrafts",

                    ProductCategory = ProductCategory.Overdraft,
                    SecurityTypes  = SecurityType.None,
               };


               var suncorpLineOfCredit = new Product
               {
                    LenderID = suncorp.LenderID,
                    Title = "Line of Credit",
                  
                    MinimumAmount = 10000,
                    MaximumAmount = 1000000,

                    RateTypes = RateType.Variable ,
                    SecurityTypes = SecurityType.ResidentialProperty | SecurityType.CommercialProperty,
                    LVR = 80,
                    LVRNotes = "Residential Property 80%, Commercial Property 70%",

                    
                    RepaymentTypes = RepaymentType.InterestInArrears,
                    EarlyRepaymentsAllowed = true,
               
                    EstablishmentFeePercentage = 0.3,
                    EstablishmentFeeMinimum = 800,
                    MonthlyAccountKeepingFee = 27,
                    TerminationFee = 250,

                    ProductCategory = ProductCategory.LineOfCredit                   

               };

               
               var suncorpTermLoan1 = new Product
               {
                    LenderID = suncorp.LenderID,
                    Title = "Term Loan Fixed",
                    MinimumAmount = 10000,
                    MaximumAmount = 1000000,

                    MinimumDuration = 1,
                    MaximumDuration = 25,
                    MaximumDurationNotes = "30 years (residential security), 15 years (commercial security)",

                    RateTypes = RateType.Fixed,
                    
                    SecurityTypes = SecurityType.ResidentialProperty | SecurityType.CommercialProperty,

                    LVR = 80,
                    LVRNotes = "Residential Property 80%, Commercial Property 70%, Low Doc 60%",


                    RepaymentFrequencies = FrequencyType.Weekly | FrequencyType.Fortnightly | FrequencyType.Monthly ,
                    RepaymentTypes = RepaymentType.PrincipalPlusInterest | RepaymentType.InterestInArrears | RepaymentType.InterestInAdvance,

                    ProgressDrawingsAllowed = false,
                    EarlyRepaymentsAllowed = true,

                    EstablishmentFeePercentage = 0.3,
                    EstablishmentFeeMinimum = 800,
                    MonthlyAccountKeepingFee = 15,
                    
                    TerminationFee = 250,


                    ProductCategory = ProductCategory.TermLoan

               };

               var suncorpTermLoan2 = new Product
               {
                    LenderID = suncorp.LenderID,
                    Title = "Term Loan",
                    MinimumAmount = 10000,
                    MaximumAmount = 1000000,

                    MinimumDuration = 1,
                    MaximumDuration = 25,
                    MaximumDurationNotes = "30 years (residential security), 15 years (commercial security)",

                    RateTypes = RateType.Variable,
                    SecurityTypes = SecurityType.ResidentialProperty | SecurityType.CommercialProperty,

                    LVR = 80,
                    LVRNotes = "Residential Property 80%, Commercial Property 70%, Low Doc 60%",

                    RepaymentFrequencies = FrequencyType.Weekly | FrequencyType.Fortnightly | FrequencyType.Monthly ,
                    RepaymentTypes = RepaymentType.PrincipalPlusInterest | RepaymentType.InterestInArrears | RepaymentType.InterestInAdvance,

                    ProgressDrawingsAllowed = true,
                    EarlyRepaymentsAllowed = true,

                    EstablishmentFeePercentage = 0.3,
                    EstablishmentFeeMinimum = 800,
                    MonthlyAccountKeepingFee = 15,

                    TerminationFee = 250,
                    

                    Comments = "Add 0.15% to interest rate for Low Doc loan",


                    ProductCategory = ProductCategory.TermLoan

               };

               var suncorpTermLoan3 = new Product
               {
                    LenderID = suncorp.LenderID,
                    Title = "Business Essentials",
                   

                    RateTypes = RateType.Variable,
                    SecurityTypes = SecurityType.ResidentialProperty | SecurityType.CommercialProperty,

                    RepaymentFrequencies = FrequencyType.Weekly | FrequencyType.Fortnightly | FrequencyType.Monthly,
                    RepaymentTypes = RepaymentType.PrincipalPlusInterest | RepaymentType.InterestInArrears,
                    LVR = 80,

                    EstablishmentFeePercentage = 0.5,
                    EstablishmentFeeMinimum = 700,
                    MonthlyAccountKeepingFee = 0,
                    TerminationFee = 250,
                    ProgressDrawingsAllowed = true,
                    ProductCategory = ProductCategory.TermLoan

               };
           

               context.Products.Add(suncorpOverdraft);
               context.Products.Add(suncorpLineOfCredit);
               context.Products.Add(suncorpTermLoan1);
               context.Products.Add(suncorpTermLoan2);
               context.Products.Add(suncorpTermLoan3);



               #endregion

          }
     }
}
