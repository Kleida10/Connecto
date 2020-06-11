namespace Co_nnecto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeAgain3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ApplicationUserStudentParents", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.ApplicationUserStudentParents", "StudentParent_ID", "dbo.StudentParents");
            DropIndex("dbo.ApplicationUserStudentParents", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.ApplicationUserStudentParents", new[] { "StudentParent_ID" });
            AlterColumn("dbo.StudentParents", "ParentID", c => c.String(maxLength: 128));
            CreateIndex("dbo.StudentParents", "ParentID");
            AddForeignKey("dbo.StudentParents", "ParentID", "dbo.AspNetUsers", "Id");
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
            
            DropForeignKey("dbo.StudentParents", "ParentID", "dbo.AspNetUsers");
            DropIndex("dbo.StudentParents", new[] { "ParentID" });
            AlterColumn("dbo.StudentParents", "ParentID", c => c.String());
            CreateIndex("dbo.ApplicationUserStudentParents", "StudentParent_ID");
            CreateIndex("dbo.ApplicationUserStudentParents", "ApplicationUser_Id");
            AddForeignKey("dbo.ApplicationUserStudentParents", "StudentParent_ID", "dbo.StudentParents", "ID", cascadeDelete: true);
            AddForeignKey("dbo.ApplicationUserStudentParents", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
