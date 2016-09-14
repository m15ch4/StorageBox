namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SBTasks", "DateAdded", c => c.DateTime());
            AlterColumn("dbo.SBTasks", "DateEnded", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SBTasks", "DateEnded", c => c.DateTime(nullable: false));
            AlterColumn("dbo.SBTasks", "DateAdded", c => c.DateTime(nullable: false));
        }
    }
}
