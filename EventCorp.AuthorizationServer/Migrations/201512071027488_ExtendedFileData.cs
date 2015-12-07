namespace EventCorp.AuthorizationServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExtendedFileData : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserFiles", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.UserFiles", "Global", c => c.Boolean(nullable: false));
            CreateIndex("dbo.UserFiles", "UserId");
            AddForeignKey("dbo.UserFiles", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserFiles", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserFiles", new[] { "UserId" });
            DropColumn("dbo.UserFiles", "Global");
            DropColumn("dbo.UserFiles", "UserId");
        }
    }
}
