namespace SimpleMoney.Migrations
{
     using System;
     using System.Data.Entity;
     using System.Data.Entity.Migrations;
     using System.Linq;
     using System.Web.Security;
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
                    WebSecurity.CreateUserAndAccount("vamsi.mg@gmail.com", "helloworld", new { Mobile = "0432100092" });
               }

               if (!Roles.GetRolesForUser("vamsi.mg@gmail.com").Contains("admin"))
               {                    
                    Roles.AddUsersToRoles(new[] {"vamsi.mg@gmail.com"}, new[] {"admin"});
               }

          }
     }
}
