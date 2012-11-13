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
               lenders.Add("Members Equity");
               lenders.Add("Bendigo Bank");


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

               foreach(var lender in currentLenders)
               {
                    if (lender.Products.Count == 0) 
                    {
                         var newEmployee = new LenderEmployee();

                         newEmployee.LenderID = lender.LenderID;
                         newEmployee.UserID = lenderUserID;
                         newEmployee.Position = "Approver";


                         var newProduct1 = new Product();

                         newProduct1.LenderID = lender.LenderID;
                         newProduct1.Title = FinanceTypes.MortgageLoan.ToString();
                         newProduct1.Terms = "Monthly Payments";
                         newProduct1.MinimumAmount = 10000;
                         newProduct1.MaximumAmount = 100000;
                         newProduct1.Type = FinanceTypes.MortgageLoan;
                         newProduct1.InterestRate = 6;
                         newProduct1.IsVariableRate = false;

                         var newProduct2 = new Product();

                         newProduct2.LenderID = lender.LenderID;
                         newProduct2.Title = FinanceTypes.Overdraft.ToString();
                         newProduct2.Terms = "Ongoing";
                         newProduct2.MinimumAmount = 1000;
                         newProduct2.MaximumAmount = 20000;
                         newProduct2.Type = FinanceTypes.Overdraft;
                         newProduct2.InterestRate = 15;
                         newProduct2.IsVariableRate = true;

                         var newProduct3 = new Product();

                         newProduct3.LenderID = lender.LenderID;
                         newProduct3.Title = FinanceTypes.LineOfCredit.ToString();
                         newProduct3.Terms = "Ongoing";
                         newProduct3.MinimumAmount = 1000;
                         newProduct3.MaximumAmount = 30000;
                         newProduct3.Type = FinanceTypes.LineOfCredit;
                         newProduct3.InterestRate = 15;
                         newProduct3.IsVariableRate = true;

                         var newProduct4 = new Product();

                         newProduct4.LenderID = lender.LenderID;
                         newProduct4.Title = FinanceTypes.EquipmentLoan.ToString();
                         newProduct4.Terms = "Monthly Payments";
                         newProduct4.MinimumAmount = 10000;
                         newProduct4.MaximumAmount = 100000;
                         newProduct4.Type = FinanceTypes.EquipmentLoan;
                         newProduct4.InterestRate = 10;
                         newProduct4.IsVariableRate = true;

                         context.Products.Add(newProduct1);
                         context.Products.Add(newProduct2);
                         context.Products.Add(newProduct3);
                         context.Products.Add(newProduct4);

                         context.LenderEmployees.Add(newEmployee);
                    }

               }

               context.SaveChanges();
          }
     }
}
