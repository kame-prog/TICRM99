namespace TICRM.UI.ASPNetMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datatpechange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "CompanyId", c => c.Guid(nullable: false));
            DropColumn("dbo.AspNetUsers", "Company");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Company", c => c.String());
            DropColumn("dbo.AspNetUsers", "CompanyId");
        }
    }
}
