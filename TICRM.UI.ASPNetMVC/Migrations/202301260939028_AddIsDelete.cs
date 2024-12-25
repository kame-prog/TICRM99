namespace TICRM.UI.ASPNetMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsDelete : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "IsDelete", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "IsDelete", c => c.String());
        }
    }
}
