namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Removedboxstatuses : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Boxes", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Boxes", "Status", c => c.Int(nullable: false));
        }
    }
}
