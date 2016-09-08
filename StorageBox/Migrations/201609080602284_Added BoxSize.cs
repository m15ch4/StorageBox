namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedBoxSize : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoxSizes",
                c => new
                    {
                        BoxSizeID = c.Int(nullable: false, identity: true),
                        BoxSizeName = c.String(),
                    })
                .PrimaryKey(t => t.BoxSizeID);
            
            AddColumn("dbo.Boxes", "BoxSize_BoxSizeID", c => c.Int());
            AlterColumn("dbo.Boxes", "AddressRow", c => c.Short(nullable: false));
            AlterColumn("dbo.Boxes", "AddressCol", c => c.Short(nullable: false));
            CreateIndex("dbo.Boxes", "BoxSize_BoxSizeID");
            AddForeignKey("dbo.Boxes", "BoxSize_BoxSizeID", "dbo.BoxSizes", "BoxSizeID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Boxes", "BoxSize_BoxSizeID", "dbo.BoxSizes");
            DropIndex("dbo.Boxes", new[] { "BoxSize_BoxSizeID" });
            AlterColumn("dbo.Boxes", "AddressCol", c => c.Byte(nullable: false));
            AlterColumn("dbo.Boxes", "AddressRow", c => c.Byte(nullable: false));
            DropColumn("dbo.Boxes", "BoxSize_BoxSizeID");
            DropTable("dbo.BoxSizes");
        }
    }
}
