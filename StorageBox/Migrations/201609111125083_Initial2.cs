namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SBUsers",
                c => new
                    {
                        SBUserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        RFID = c.String(),
                    })
                .PrimaryKey(t => t.SBUserID);
            
            AddColumn("dbo.SBTasks", "Box_BoxID", c => c.Int());
            AddColumn("dbo.SBTasks", "ProductSKU_ProductID", c => c.Int());
            AddColumn("dbo.SBTasks", "ProductSKU_ProductSKUID", c => c.Int());
            AddColumn("dbo.SBTasks", "SBUser_SBUserID", c => c.Int());
            CreateIndex("dbo.SBTasks", "Box_BoxID");
            CreateIndex("dbo.SBTasks", new[] { "ProductSKU_ProductID", "ProductSKU_ProductSKUID" });
            CreateIndex("dbo.SBTasks", "SBUser_SBUserID");
            AddForeignKey("dbo.SBTasks", "Box_BoxID", "dbo.Boxes", "BoxID");
            AddForeignKey("dbo.SBTasks", new[] { "ProductSKU_ProductID", "ProductSKU_ProductSKUID" }, "dbo.ProductSKUs", new[] { "ProductID", "ProductSKUID" });
            AddForeignKey("dbo.SBTasks", "SBUser_SBUserID", "dbo.SBUsers", "SBUserID");
            DropColumn("dbo.SBTasks", "BoxID");
            DropColumn("dbo.SBTasks", "ProductSKUID");
            DropColumn("dbo.SBTasks", "ProductID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SBTasks", "ProductID", c => c.Int(nullable: false));
            AddColumn("dbo.SBTasks", "ProductSKUID", c => c.Int(nullable: false));
            AddColumn("dbo.SBTasks", "BoxID", c => c.Int(nullable: false));
            DropForeignKey("dbo.SBTasks", "SBUser_SBUserID", "dbo.SBUsers");
            DropForeignKey("dbo.SBTasks", new[] { "ProductSKU_ProductID", "ProductSKU_ProductSKUID" }, "dbo.ProductSKUs");
            DropForeignKey("dbo.SBTasks", "Box_BoxID", "dbo.Boxes");
            DropIndex("dbo.SBTasks", new[] { "SBUser_SBUserID" });
            DropIndex("dbo.SBTasks", new[] { "ProductSKU_ProductID", "ProductSKU_ProductSKUID" });
            DropIndex("dbo.SBTasks", new[] { "Box_BoxID" });
            DropColumn("dbo.SBTasks", "SBUser_SBUserID");
            DropColumn("dbo.SBTasks", "ProductSKU_ProductSKUID");
            DropColumn("dbo.SBTasks", "ProductSKU_ProductID");
            DropColumn("dbo.SBTasks", "Box_BoxID");
            DropTable("dbo.SBUsers");
        }
    }
}
