namespace StorageBox.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adduserroleasseparatemodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SBRoles",
                c => new
                    {
                        SBRoleID = c.Int(nullable: false, identity: true),
                        SBRoleName = c.String(),
                    })
                .PrimaryKey(t => t.SBRoleID);
            
            AddColumn("dbo.SBUsers", "Role_SBRoleID", c => c.Int());
            CreateIndex("dbo.SBUsers", "Role_SBRoleID");
            AddForeignKey("dbo.SBUsers", "Role_SBRoleID", "dbo.SBRoles", "SBRoleID");
            DropColumn("dbo.SBUsers", "Role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SBUsers", "Role", c => c.Int(nullable: false));
            DropForeignKey("dbo.SBUsers", "Role_SBRoleID", "dbo.SBRoles");
            DropIndex("dbo.SBUsers", new[] { "Role_SBRoleID" });
            DropColumn("dbo.SBUsers", "Role_SBRoleID");
            DropTable("dbo.SBRoles");
        }
    }
}
