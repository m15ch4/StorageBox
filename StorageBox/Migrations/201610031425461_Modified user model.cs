namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modifiedusermodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SBUsers", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SBUsers", "IsActive");
        }
    }
}
