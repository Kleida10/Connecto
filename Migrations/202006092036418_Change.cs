namespace Co_nnecto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StudentApplicationUsers", "Student_ID", "dbo.Students");
            DropForeignKey("dbo.StudentApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.StudentApplicationUsers", new[] { "Student_ID" });
            DropIndex("dbo.StudentApplicationUsers", new[] { "ApplicationUser_Id" });
            AddColumn("dbo.Students", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Students", "ApplicationUser_Id");
            AddForeignKey("dbo.Students", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
            DropTable("dbo.StudentApplicationUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StudentApplicationUsers",
                c => new
                    {
                        Student_ID = c.Int(nullable: false),
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Student_ID, t.ApplicationUser_Id });
            
            DropForeignKey("dbo.Students", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Students", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.Students", "ApplicationUser_Id");
            CreateIndex("dbo.StudentApplicationUsers", "ApplicationUser_Id");
            CreateIndex("dbo.StudentApplicationUsers", "Student_ID");
            AddForeignKey("dbo.StudentApplicationUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.StudentApplicationUsers", "Student_ID", "dbo.Students", "ID", cascadeDelete: true);
        }
    }
}
