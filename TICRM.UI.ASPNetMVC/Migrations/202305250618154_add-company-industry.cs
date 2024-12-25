namespace TICRM.UI.ASPNetMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcompanyindustry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Countryid", c => c.Guid(nullable: false));
            AddColumn("dbo.AspNetUsers", "Industryid", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Industryid");
            DropColumn("dbo.AspNetUsers", "Countryid");
        }
    }
}
