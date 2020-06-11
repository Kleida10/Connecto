namespace Co_nnecto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveStudentParentClass : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserStudentParents", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserStudentParents", "StudentParent_ID", "dbo.StudentParents");
            DropForeignKey("dbo.StudentParents", "StudentID", "dbo.Students");
            DropIndex("dbo.StudentParents", new[] { "StudentID" });
            DropIndex("dbo.ApplicationUserStudentParents", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserStudentParents", new[] { "StudentParent_ID" });
            CreateTable(
                "dbo.ApplicationUserStudents",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        Student_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.Student_ID })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Students", t => t.Student_ID, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.Student_ID);
            
            DropTable("dbo.StudentParents");
            DropTable("dbo.ApplicationUserStudentParents");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ApplicationUserStudentParents",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        StudentParent_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.StudentParent_ID });
            
            CreateTable(
                "dbo.StudentParents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        ParentID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.ApplicationUserStudents", "Student_ID", "dbo.Students");
            DropForeignKey("dbo.ApplicationUserStudents", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserStudents", new[] { "Student_ID" });
            DropIndex("dbo.ApplicationUserStudents", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserStudents");
            CreateIndex("dbo.ApplicationUserStudentParents", "StudentParent_ID");
            CreateIndex("dbo.ApplicationUserStudentParents", "ApplicationUser_Id");
            CreateIndex("dbo.StudentParents", "StudentID");
            AddForeignKey("dbo.StudentParents", "StudentID", "dbo.Students", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserStudentParents", "StudentParent_ID", "dbo.StudentParents", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserStudentParents", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
