namespace Co_nnecto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        ParentsID = c.Int(nullable: false),
                        TeachersID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.AspNetUsers", "Student_ID", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Student_ID1", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Student_ID");
            CreateIndex("dbo.AspNetUsers", "Student_ID1");
            AddForeignKey("dbo.AspNetUsers", "Student_ID", "dbo.Students", "ID");
            AddForeignKey("dbo.AspNetUsers", "Student_ID1", "dbo.Students", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Student_ID1", "dbo.Students");
            DropForeignKey("dbo.AspNetUsers", "Student_ID", "dbo.Students");
            DropIndex("dbo.AspNetUsers", new[] { "Student_ID1" });
            DropIndex("dbo.AspNetUsers", new[] { "Student_ID" });
            DropColumn("dbo.AspNetUsers", "Student_ID1");
            DropColumn("dbo.AspNetUsers", "Student_ID");
            DropTable("dbo.Students");
        }
    }
}
