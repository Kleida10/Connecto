namespace Co_nnecto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovingSP : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.StudentParents", "ParentID", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentParents", "StudentID", "dbo.Students");
            DropIndex("dbo.StudentParents", new[] { "StudentID" });
            DropIndex("dbo.StudentParents", new[] { "ParentID" });
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
            
            DropTable("dbo.StudentParents");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.StudentParents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        StudentID = c.Int(nullable: false),
                        ParentID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.StudentParent", "ParentID", "dbo.AspNetUsers");
            DropForeignKey("dbo.StudentParent", "StudentID", "dbo.Students");
            DropIndex("dbo.StudentParent", new[] { "ParentID" });
            DropIndex("dbo.StudentParent", new[] { "StudentID" });
            DropTable("dbo.StudentParent");
            CreateIndex("dbo.StudentParents", "ParentID");
            CreateIndex("dbo.StudentParents", "StudentID");
            AddForeignKey("dbo.StudentParents", "StudentID", "dbo.Students", "ID", cascadeDelete: true);
            AddForeignKey("dbo.StudentParents", "ParentID", "dbo.AspNetUsers", "Id");
        }
    }
}
