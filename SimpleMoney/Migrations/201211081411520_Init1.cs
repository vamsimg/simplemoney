namespace SimpleMoney.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Applications", "Status", c => c.Int(nullable: false));
            AlterColumn("dbo.ApplicationSubmissions", "Status", c => c.Int(nullable: false));
            DropColumn("dbo.Applications", "ReadyForMatching");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Applications", "ReadyForMatching", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ApplicationSubmissions", "Status", c => c.String());
            DropColumn("dbo.Applications", "Status");
        }
    }
}
