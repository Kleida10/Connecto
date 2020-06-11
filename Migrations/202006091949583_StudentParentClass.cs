namespace Co_nnecto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentParentClass : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ApplicationUserStudents", newName: "StudentApplicationUsers");
            DropPrimaryKey("dbo.StudentApplicationUsers");
            CreateTable(
                "dbo.StudentParents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        ParentID = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Students", t => t.StudentID, cascadeDelete: true)
                .Index(t => t.StudentID);
            
            AddColumn("dbo.AspNetUsers", "StudentParent_ID", c => c.Int());
            AddPrimaryKey("dbo.StudentApplicationUsers", new[] { "Student_ID", "ApplicationUser_Id" });
            CreateIndex("dbo.AspNetUsers", "StudentParent_ID");
            AddForeignKey("dbo.AspNetUsers", "StudentParent_ID", "dbo.StudentParents", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentParents", "StudentID", "dbo.Students");
            DropForeignKey("dbo.AspNetUsers", "StudentParent_ID", "dbo.StudentParents");
            DropIndex("dbo.AspNetUsers", new[] { "StudentParent_ID" });
            DropIndex("dbo.StudentParents", new[] { "StudentID" });
            DropPrimaryKey("dbo.StudentApplicationUsers");
            DropColumn("dbo.AspNetUsers", "StudentParent_ID");
            DropTable("dbo.StudentParents");
            AddPrimaryKey("dbo.StudentApplicationUsers", new[] { "ApplicationUser_Id", "Student_ID" });
            RenameTable(name: "dbo.StudentApplicationUsers", newName: "ApplicationUserStudents");
        }
    }
}
