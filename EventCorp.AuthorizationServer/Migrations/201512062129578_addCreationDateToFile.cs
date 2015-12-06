namespace EventCorp.AuthorizationServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCreationDateToFile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserFiles", "Created", c => c.DateTime(nullable: false));
            AlterColumn("dbo.UserFiles", "Id", c => c.Guid(nullable: false, identity: true, defaultValueSql: "newid()"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserFiles", "Created");
        }
    }
}
