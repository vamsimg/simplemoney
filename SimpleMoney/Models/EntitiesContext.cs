using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SimpleMoney.Models
{
     public class EntitiesContext : DbContext
     {
          public EntitiesContext()
               : base("DefaultConnection")
          {
          }

          protected override void OnModelCreating(DbModelBuilder modelBuilder)
          {
              Database.SetInitializer<EntitiesContext>(new CreateDatabaseIfNotExists<EntitiesContext>());
          }

          public DbSet<UserProfile> UserProfiles { get; set; }
          public DbSet<Lender> Lenders { get; set; }
          public DbSet<LenderEmployee> LenderEmployees { get; set; }
          public DbSet<Product> Products { get; set; }

          public DbSet<Application> Applications { get; set; }
          public DbSet<ApplicationBacker> ApplicationBackers { get; set; }
          public DbSet<UserApplication> UserApplications { get; set; }


          public DbSet<ApplicationSubmission> ApplicationSubmissions { get; set; }
          public DbSet<Asset> Assets { get; set; }
          public DbSet<Liability> Liabilities { get; set; }
     }

     
}