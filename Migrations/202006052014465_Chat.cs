namespace Co_nnecto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Chat : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false),
                        MessageToPost = c.String(nullable: false),
                        From = c.String(),
                        DatePosted = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Replies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageId = c.Int(nullable: false),
                        ReplyFrom = c.String(),
                        ReplyMessage = c.String(nullable: false),
                        ReplyDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Replies");
            DropTable("dbo.Messages");
        }
    }
}
