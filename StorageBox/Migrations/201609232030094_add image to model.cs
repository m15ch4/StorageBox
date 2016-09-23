namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addimagetomodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ProductImageContent", c => c.Binary());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "ProductImageContent");
        }
    }
}
