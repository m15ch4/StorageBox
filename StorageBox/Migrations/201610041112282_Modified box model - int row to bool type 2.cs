namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modifiedboxmodelintrowtobooltype2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SBTasks", "DateStarted", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SBTasks", "DateStarted");
        }
    }
}
