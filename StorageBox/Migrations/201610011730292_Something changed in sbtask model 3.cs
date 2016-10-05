namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Somethingchangedinsbtaskmodel3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SBTasks", "SetStatus", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SBTasks", "SetStatus");
        }
    }
}
