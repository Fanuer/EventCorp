namespace EventCorp.AuthorizationServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOneToManyToUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "City", c => c.String(nullable: false));
            AddColumn("dbo.AspNetUsers", "GenderType", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "FavoriteEventType", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "AvatarId", c => c.Guid(nullable: true));
            AlterColumn("dbo.AspNetUsers", "Surname", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Forename", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Forename", c => c.String());
            AlterColumn("dbo.AspNetUsers", "Surname", c => c.String());
            DropColumn("dbo.AspNetUsers", "AvatarId");
            DropColumn("dbo.AspNetUsers", "FavoriteEventType");
            DropColumn("dbo.AspNetUsers", "GenderType");
            DropColumn("dbo.AspNetUsers", "City");
        }
    }
}
