namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedSBTaskCreatedIsValidfieldImplementedvalidationofitemget : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SBTasks", "IsValid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SBTasks", "IsValid");
        }
    }
}
