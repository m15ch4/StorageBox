namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class taskprocess : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Boxes", name: "ProductID", newName: "ProductSKU_ProductID");
            RenameColumn(table: "dbo.Boxes", name: "ProductSKUID", newName: "ProductSKU_ProductSKUID");
            RenameIndex(table: "dbo.Boxes", name: "IX_ProductID_ProductSKUID", newName: "IX_ProductSKU_ProductID_ProductSKU_ProductSKUID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Boxes", name: "IX_ProductSKU_ProductID_ProductSKU_ProductSKUID", newName: "IX_ProductID_ProductSKUID");
            RenameColumn(table: "dbo.Boxes", name: "ProductSKU_ProductSKUID", newName: "ProductSKUID");
            RenameColumn(table: "dbo.Boxes", name: "ProductSKU_ProductID", newName: "ProductID");
        }
    }
}
