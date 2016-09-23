namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedboxstatuses : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Boxes", "Status", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Boxes", "Status");
        }
    }
}
