namespace Co_nnecto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Student_ID", "dbo.Students");
            DropForeignKey("dbo.AspNetUsers", "Student_ID1", "dbo.Students");
            DropIndex("dbo.AspNetUsers", new[] { "Student_ID" });
            DropIndex("dbo.AspNetUsers", new[] { "Student_ID1" });
            CreateTable(
                "dbo.StudentParent",
                c => new
                    {
                        StudentID = c.Int(nullable: false),
                        ParentID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.StudentID, t.ParentID })
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ParentID, cascadeDelete: true)
                .Index(t => t.StudentID)
                .Index(t => t.ParentID);
            
            CreateTable(
                "dbo.StudentTeacher",
                c => new
                    {
                        StudentID = c.Int(nullable: false),
                        TeacherID = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.StudentID, t.TeacherID })
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.TeacherID, cascadeDelete: true)
                .Index(t => t.StudentID)
                .Index(t => t.TeacherID);
            
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
            DropForeignKey("dbo.StudentTeacher", "TeacherID", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentTeacher", "StudentID", "dbo.Students");
            DropForeignKey("dbo.StudentParent", "ParentID", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentParent", "StudentID", "dbo.Students");
            DropIndex("dbo.StudentTeacher", new[] { "TeacherID" });
            DropIndex("dbo.StudentTeacher", new[] { "StudentID" });
            DropIndex("dbo.StudentParent", new[] { "ParentID" });
            DropIndex("dbo.StudentParent", new[] { "StudentID" });
            DropTable("dbo.StudentTeacher");
            DropTable("dbo.StudentParent");
            CreateIndex("dbo.AspNetUsers", "Student_ID1");
            CreateIndex("dbo.AspNetUsers", "Student_ID");
            AddForeignKey("dbo.AspNetUsers", "Student_ID1", "dbo.Students", "ID");
            AddForeignKey("dbo.AspNetUsers", "Student_ID", "dbo.Students", "ID");
        }
    }
}
