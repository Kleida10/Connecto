namespace Co_nnecto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentTeacher : DbMigration
    {
        public override void Up()
        {
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentTeacher", "TeacherID", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentTeacher", "StudentID", "dbo.Students");
            DropIndex("dbo.StudentTeacher", new[] { "TeacherID" });
            DropIndex("dbo.StudentTeacher", new[] { "StudentID" });
            DropTable("dbo.StudentTeacher");
        }
    }
}
