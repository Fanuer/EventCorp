namespace EventCorp.AuthorizationServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterCreationDateToFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserFiles", "CreatedUTC", c => c.DateTime(nullable: false));
            DropColumn("dbo.UserFiles", "Created");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserFiles", "Created", c => c.DateTime(nullable: false));
            DropColumn("dbo.UserFiles", "CreatedUTC");
        }
    }
}
