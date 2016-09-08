namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialWithoutTasks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Boxes",
                c => new
                    {
                        ProductID = c.Int(),
                        ProductSKUID = c.Int(),
                        BoxID = c.Int(nullable: false, identity: true),
                        AddressRow = c.Byte(nullable: false),
                        AddressCol = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.BoxID)
                .ForeignKey("dbo.ProductSKUs", t => new { t.ProductID, t.ProductSKUID })
                .Index(t => new { t.ProductID, t.ProductSKUID });
            
            CreateTable(
                "dbo.ProductSKUs",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        ProductSKUID = c.Int(nullable: false),
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
                        OptionID = c.Int(nullable: false),
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
                        OptionValueID = c.Int(nullable: false),
                        ValueName = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => new { t.ProductID, t.OptionID, t.OptionValueID })
                .ForeignKey("dbo.Options", t => new { t.ProductID, t.OptionID }, cascadeDelete: true)
                .Index(t => new { t.ProductID, t.OptionID, t.ValueName }, unique: true, name: "IX_OptionValues");
            
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
                .ForeignKey("dbo.ProductSKUs", t => new { t.ProductID, t.ProductSKUID }, cascadeDelete: true)
                .Index(t => new { t.ProductID, t.OptionID })
                .Index(t => new { t.ProductID, t.OptionID, t.OptionValueID })
                .Index(t => new { t.ProductID, t.ProductSKUID });
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Boxes", new[] { "ProductID", "ProductSKUID" }, "dbo.ProductSKUs");
            DropForeignKey("dbo.SKUValues", new[] { "ProductID", "ProductSKUID" }, "dbo.ProductSKUs");
            DropForeignKey("dbo.SKUValues", new[] { "ProductID", "OptionID", "OptionValueID" }, "dbo.OptionValues");
            DropForeignKey("dbo.SKUValues", new[] { "ProductID", "OptionID" }, "dbo.Options");
            DropForeignKey("dbo.ProductSKUs", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Options", "ProductID", "dbo.Products");
            DropForeignKey("dbo.OptionValues", new[] { "ProductID", "OptionID" }, "dbo.Options");
            DropForeignKey("dbo.Products", "Category_CategoryID", "dbo.Categories");
            DropIndex("dbo.SKUValues", new[] { "ProductID", "ProductSKUID" });
            DropIndex("dbo.SKUValues", new[] { "ProductID", "OptionID", "OptionValueID" });
            DropIndex("dbo.SKUValues", new[] { "ProductID", "OptionID" });
            DropIndex("dbo.OptionValues", "IX_OptionValues");
            DropIndex("dbo.Options", "IX_Options");
            DropIndex("dbo.Products", new[] { "Category_CategoryID" });
            DropIndex("dbo.Products", new[] { "ProductName" });
            DropIndex("dbo.ProductSKUs", new[] { "ProductSKUID" });
            DropIndex("dbo.ProductSKUs", new[] { "ProductID" });
            DropIndex("dbo.Boxes", new[] { "ProductID", "ProductSKUID" });
            DropTable("dbo.SKUValues");
            DropTable("dbo.OptionValues");
            DropTable("dbo.Options");
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.ProductSKUs");
            DropTable("dbo.Boxes");
        }
    }
}
