namespace SimpleMoney.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applications", "YearlyIncomeBeforeTax", c => c.Int(nullable: false));
            AddColumn("dbo.Applications", "YearlyDepreciation", c => c.Int(nullable: false));
            AddColumn("dbo.Applications", "YearlyInterestEarned", c => c.Int(nullable: false));
            AddColumn("dbo.Applications", "YearlyNetProfit", c => c.Int(nullable: false));
            AddColumn("dbo.Applications", "LoanAmount", c => c.Int(nullable: false));
            AddColumn("dbo.Applications", "LoanPurpose", c => c.String());
            AddColumn("dbo.Applications", "LoanDurationMonths", c => c.Int(nullable: false));
            AddColumn("dbo.Applications", "RepaymentTerms", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Applications", "RepaymentTerms");
            DropColumn("dbo.Applications", "LoanDurationMonths");
            DropColumn("dbo.Applications", "LoanPurpose");
            DropColumn("dbo.Applications", "LoanAmount");
            DropColumn("dbo.Applications", "YearlyNetProfit");
            DropColumn("dbo.Applications", "YearlyInterestEarned");
            DropColumn("dbo.Applications", "YearlyDepreciation");
            DropColumn("dbo.Applications", "YearlyIncomeBeforeTax");
        }
    }
}
