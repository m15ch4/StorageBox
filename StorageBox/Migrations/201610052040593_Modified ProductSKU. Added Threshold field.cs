namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedProductSKUAddedThresholdfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductSKUs", "Threshold", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductSKUs", "Threshold");
        }
    }
}
