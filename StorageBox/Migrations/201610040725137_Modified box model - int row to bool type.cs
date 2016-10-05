namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Modifiedboxmodelintrowtobooltype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Boxes", "AddressRow", c => c.Byte(nullable: false));
            AlterColumn("dbo.Boxes", "AddressCol", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Boxes", "AddressCol", c => c.Short(nullable: false));
            AlterColumn("dbo.Boxes", "AddressRow", c => c.Short(nullable: false));
        }
    }
}
