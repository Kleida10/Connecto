namespace Co_nnecto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update4 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.StudentParent", new[] { "StudentID" });
            DropIndex("dbo.StudentParent", new[] { "ParentID" });
            RenameColumn(table: "dbo.StudentParent", name: "StudentID", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.StudentParent", name: "ParentID", newName: "StudentID");
            RenameColumn(table: "dbo.StudentParent", name: "__mig_tmp__0", newName: "ParentID");
            DropPrimaryKey("dbo.StudentParent");
            AlterColumn("dbo.StudentParent", "StudentID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.StudentParent", "ParentID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.StudentParent", new[] { "ParentID", "StudentID" });
            CreateIndex("dbo.StudentParent", "ParentID");
            CreateIndex("dbo.StudentParent", "StudentID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.StudentParent", new[] { "StudentID" });
            DropIndex("dbo.StudentParent", new[] { "ParentID" });
            DropPrimaryKey("dbo.StudentParent");
            AlterColumn("dbo.StudentParent", "ParentID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.StudentParent", "StudentID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.StudentParent", new[] { "StudentID", "ParentID" });
            RenameColumn(table: "dbo.StudentParent", name: "ParentID", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.StudentParent", name: "StudentID", newName: "ParentID");
            RenameColumn(table: "dbo.StudentParent", name: "__mig_tmp__0", newName: "StudentID");
            CreateIndex("dbo.StudentParent", "ParentID");
            CreateIndex("dbo.StudentParent", "StudentID");
        }
    }
}
