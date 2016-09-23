namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Boxes",
                c => new
                    {
                        BoxID = c.Int(nullable: false, identity: true),
                        AddressRow = c.Short(nullable: false),
                        AddressCol = c.Short(nullable: false),
                        Status = c.Int(nullable: false),
                        BoxSize_BoxSizeID = c.Int(),
                        ProductSKU_ProductID = c.Int(),
                        ProductSKU_ProductSKUID = c.Int(),
                    })
                .PrimaryKey(t => t.BoxID)
                .ForeignKey("dbo.BoxSizes", t => t.BoxSize_BoxSizeID)
                .ForeignKey("dbo.ProductSKUs", t => new { t.ProductSKU_ProductID, t.ProductSKU_ProductSKUID })
                .Index(t => t.BoxSize_BoxSizeID)
                .Index(t => new { t.ProductSKU_ProductID, t.ProductSKU_ProductSKUID });
            
            CreateTable(
                "dbo.BoxSizes",
                c => new
                    {
                        BoxSizeID = c.Int(nullable: false, identity: true),
                        BoxSizeName = c.String(),
                    })
                .PrimaryKey(t => t.BoxSizeID);
            
            CreateTable(
                "dbo.ProductSKUs",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        ProductSKUID = c.Int(nullable: false, identity: true),
                        Sku = c.String(),
                        Price = c.String(),
                    })
                .PrimaryKey(t => new { t.ProductID, t.ProductSKUID })
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.ProductSKUID, unique: true);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        ProductName = c.String(maxLength: 200),
                        ProductDescription = c.String(),
                        ProductImage = c.String(),
                        Category_CategoryID = c.Int(),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Categories", t => t.Category_CategoryID)
                .Index(t => t.ProductName, unique: true)
                .Index(t => t.Category_CategoryID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Options",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        OptionID = c.Int(nullable: false, identity: true),
                        OptionName = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => new { t.ProductID, t.OptionID })
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => new { t.OptionName, t.ProductID }, unique: true, name: "IX_Options");
            
            CreateTable(
                "dbo.OptionValues",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        OptionID = c.Int(nullable: false),
                        OptionValueID = c.Int(nullable: false, identity: true),
                        ValueName = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => new { t.ProductID, t.OptionID, t.OptionValueID })
                .ForeignKey("dbo.Options", t => new { t.ProductID, t.OptionID }, cascadeDelete: true)
                .Index(t => new { t.ProductID, t.OptionID, t.ValueName }, unique: true, name: "IX_OptionValues");
            
            CreateTable(
                "dbo.SBTasks",
                c => new
                    {
                        SBTaskID = c.Int(nullable: false, identity: true),
                        SBTaskStatus = c.Int(nullable: false),
                        SBTaskType = c.Int(nullable: false),
                        DateAdded = c.DateTime(),
                        DateEnded = c.DateTime(),
                        Box_BoxID = c.Int(),
                        ProductSKU_ProductID = c.Int(),
                        ProductSKU_ProductSKUID = c.Int(),
                        SBUser_SBUserID = c.Int(),
                    })
                .PrimaryKey(t => t.SBTaskID)
                .ForeignKey("dbo.Boxes", t => t.Box_BoxID)
                .ForeignKey("dbo.ProductSKUs", t => new { t.ProductSKU_ProductID, t.ProductSKU_ProductSKUID })
                .ForeignKey("dbo.SBUsers", t => t.SBUser_SBUserID)
                .Index(t => t.Box_BoxID)
                .Index(t => new { t.ProductSKU_ProductID, t.ProductSKU_ProductSKUID })
                .Index(t => t.SBUser_SBUserID);
            
            CreateTable(
                "dbo.SBUsers",
                c => new
                    {
                        SBUserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Password = c.String(),
                        RFID = c.String(),
                    })
                .PrimaryKey(t => t.SBUserID);
            
            CreateTable(
                "dbo.SKUValues",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        ProductSKUID = c.Int(nullable: false),
                        OptionID = c.Int(nullable: false),
                        OptionValueID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductID, t.ProductSKUID, t.OptionID })
                .ForeignKey("dbo.Options", t => new { t.ProductID, t.OptionID }, cascadeDelete: false)
                .ForeignKey("dbo.OptionValues", t => new { t.ProductID, t.OptionID, t.OptionValueID }, cascadeDelete: false)
                .ForeignKey("dbo.ProductSKUs", t => new { t.ProductID, t.ProductSKUID }, cascadeDelete: false)
                .Index(t => new { t.ProductID, t.OptionID })
                .Index(t => new { t.ProductID, t.OptionID, t.OptionValueID })
                .Index(t => new { t.ProductID, t.ProductSKUID });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SKUValues", new[] { "ProductID", "ProductSKUID" }, "dbo.ProductSKUs");
            DropForeignKey("dbo.SKUValues", new[] { "ProductID", "OptionID", "OptionValueID" }, "dbo.OptionValues");
            DropForeignKey("dbo.SKUValues", new[] { "ProductID", "OptionID" }, "dbo.Options");
            DropForeignKey("dbo.SBTasks", "SBUser_SBUserID", "dbo.SBUsers");
            DropForeignKey("dbo.SBTasks", new[] { "ProductSKU_ProductID", "ProductSKU_ProductSKUID" }, "dbo.ProductSKUs");
            DropForeignKey("dbo.SBTasks", "Box_BoxID", "dbo.Boxes");
            DropForeignKey("dbo.ProductSKUs", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Options", "ProductID", "dbo.Products");
            DropForeignKey("dbo.OptionValues", new[] { "ProductID", "OptionID" }, "dbo.Options");
            DropForeignKey("dbo.Products", "Category_CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Boxes", new[] { "ProductSKU_ProductID", "ProductSKU_ProductSKUID" }, "dbo.ProductSKUs");
            DropForeignKey("dbo.Boxes", "BoxSize_BoxSizeID", "dbo.BoxSizes");
            DropIndex("dbo.SKUValues", new[] { "ProductID", "ProductSKUID" });
            DropIndex("dbo.SKUValues", new[] { "ProductID", "OptionID", "OptionValueID" });
            DropIndex("dbo.SKUValues", new[] { "ProductID", "OptionID" });
            DropIndex("dbo.SBTasks", new[] { "SBUser_SBUserID" });
            DropIndex("dbo.SBTasks", new[] { "ProductSKU_ProductID", "ProductSKU_ProductSKUID" });
            DropIndex("dbo.SBTasks", new[] { "Box_BoxID" });
            DropIndex("dbo.OptionValues", "IX_OptionValues");
            DropIndex("dbo.Options", "IX_Options");
            DropIndex("dbo.Products", new[] { "Category_CategoryID" });
            DropIndex("dbo.Products", new[] { "ProductName" });
            DropIndex("dbo.ProductSKUs", new[] { "ProductSKUID" });
            DropIndex("dbo.ProductSKUs", new[] { "ProductID" });
            DropIndex("dbo.Boxes", new[] { "ProductSKU_ProductID", "ProductSKU_ProductSKUID" });
            DropIndex("dbo.Boxes", new[] { "BoxSize_BoxSizeID" });
            DropTable("dbo.SKUValues");
            DropTable("dbo.SBUsers");
            DropTable("dbo.SBTasks");
            DropTable("dbo.OptionValues");
            DropTable("dbo.Options");
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.ProductSKUs");
            DropTable("dbo.BoxSizes");
            DropTable("dbo.Boxes");
        }
    }
}
