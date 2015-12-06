namespace EventCorp.AuthorizationServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserFiles",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"),
                        ContentType = c.String(nullable: false),
                        Content = c.Binary(nullable: false),
                        Name = c.String(nullable: false),
                        IsTemp = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserFiles");
        }
    }
}
