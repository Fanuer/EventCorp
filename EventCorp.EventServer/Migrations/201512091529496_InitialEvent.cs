namespace EventCorp.EventServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialEvent : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                        MaxNumberOfParticipants = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        Place = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Subscribers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        SubscriptionTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        EventId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Events", t => t.EventId, cascadeDelete: true)
                .Index(t => new { t.UserId, t.EventId }, unique: true, name: "IX_UserToEvent");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Subscribers", "EventId", "dbo.Events");
            DropIndex("dbo.Subscribers", "IX_UserToEvent");
            DropTable("dbo.Subscribers");
            DropTable("dbo.Events");
        }
    }
}
