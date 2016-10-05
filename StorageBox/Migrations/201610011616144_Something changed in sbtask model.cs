namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Somethingchangedinsbtaskmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SBTasks", "IsNotifying", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SBTasks", "IsNotifying");
        }
    }
}
