namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedpasswordfieldtoSBusermodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SBUsers", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SBUsers", "Password");
        }
    }
}
