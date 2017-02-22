namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Zmodyfikowanomodelsbtaskabyniemialzaleznosciodinnychmodeli : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SBTasks", "CategoryName", c => c.String());
            AddColumn("dbo.SBTasks", "ProductName", c => c.String());
            AddColumn("dbo.SBTasks", "SKU", c => c.String());
            AddColumn("dbo.SBTasks", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SBTasks", "UserName");
            DropColumn("dbo.SBTasks", "SKU");
            DropColumn("dbo.SBTasks", "ProductName");
            DropColumn("dbo.SBTasks", "CategoryName");
        }
    }
}
