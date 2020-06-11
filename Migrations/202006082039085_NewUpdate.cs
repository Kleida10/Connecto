namespace Co_nnecto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Student_ID", "dbo.Students");
            DropForeignKey("dbo.AspNetUsers", "Student_ID1", "dbo.Students");
            DropIndex("dbo.AspNetUsers", new[] { "Student_ID" });
            DropIndex("dbo.AspNetUsers", new[] { "Student_ID1" });
            CreateTable(
                "dbo.StudentParents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        ParentID = c.Int(nullable: false),
                        Parent_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.Parent_Id)
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: true)
                .Index(t => t.StudentID)
                .Index(t => t.Parent_Id);
            
            DropColumn("dbo.Students", "ParentsID");
            DropColumn("dbo.Students", "TeachersID");
            DropColumn("dbo.AspNetUsers", "Student_ID");
            DropColumn("dbo.AspNetUsers", "Student_ID1");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Student_ID1", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Student_ID", c => c.Int());
            AddColumn("dbo.Students", "TeachersID", c => c.Int(nullable: false));
            AddColumn("dbo.Students", "ParentsID", c => c.Int(nullable: false));
            DropForeignKey("dbo.StudentParents", "StudentID", "dbo.Students");
            DropForeignKey("dbo.StudentParents", "Parent_Id", "dbo.AspNetUsers");
            DropIndex("dbo.StudentParents", new[] { "Parent_Id" });
            DropIndex("dbo.StudentParents", new[] { "StudentID" });
            DropTable("dbo.StudentParents");
            CreateIndex("dbo.AspNetUsers", "Student_ID1");
            CreateIndex("dbo.AspNetUsers", "Student_ID");
            AddForeignKey("dbo.AspNetUsers", "Student_ID1", "dbo.Students", "ID");
            AddForeignKey("dbo.AspNetUsers", "Student_ID", "dbo.Students", "ID");
        }
    }
}
