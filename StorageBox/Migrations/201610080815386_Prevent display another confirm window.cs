namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Preventdisplayanotherconfirmwindow : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SBTasks", "IsValid", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SBTasks", "IsValid", c => c.Boolean(nullable: false));
        }
    }
}
