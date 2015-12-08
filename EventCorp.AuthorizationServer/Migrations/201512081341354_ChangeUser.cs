namespace EventCorp.AuthorizationServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUser : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "AvatarId", c => c.Guid(nullable: true));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "AvatarId", c => c.Guid(nullable: false));
        }
    }
}
