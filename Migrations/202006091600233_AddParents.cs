namespace Co_nnecto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddParents : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StudentParents", "Parent_Id", "dbo.AspNetUsers");
            DropIndex("dbo.StudentParents", new[] { "Parent_Id" });
            CreateTable(
                "dbo.ApplicationUserStudentParents",
                c => new
                    {
                        ApplicationUser_Id = c.String(nullable: false, maxLength: 128),
                        StudentParent_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_Id, t.StudentParent_ID })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.StudentParents", t => t.StudentParent_ID, cascadeDelete: true)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.StudentParent_ID);
            
            DropColumn("dbo.StudentParents", "Parent_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.StudentParents", "Parent_Id", c => c.String(maxLength: 128));
            DropForeignKey("dbo.ApplicationUserStudentParents", "StudentParent_ID", "dbo.StudentParents");
            DropForeignKey("dbo.ApplicationUserStudentParents", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApplicationUserStudentParents", new[] { "StudentParent_ID" });
            DropIndex("dbo.ApplicationUserStudentParents", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApplicationUserStudentParents");
            CreateIndex("dbo.StudentParents", "Parent_Id");
            AddForeignKey("dbo.StudentParents", "Parent_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
