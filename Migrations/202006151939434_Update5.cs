namespace Co_nnecto.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update5 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.StudentParent", new[] { "ParentID" });
            DropIndex("dbo.StudentParent", new[] { "StudentID" });
            RenameColumn(table: "dbo.StudentParent", name: "ParentID", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.StudentParent", name: "StudentID", newName: "ParentID");
            RenameColumn(table: "dbo.StudentParent", name: "__mig_tmp__0", newName: "StudentID");
            DropPrimaryKey("dbo.StudentParent");
            AlterColumn("dbo.StudentParent", "ParentID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.StudentParent", "StudentID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.StudentParent", new[] { "StudentID", "ParentID" });
            CreateIndex("dbo.StudentParent", "StudentID");
            CreateIndex("dbo.StudentParent", "ParentID");
        }
        
        public override void Down()
        {
            DropIndex("dbo.StudentParent", new[] { "ParentID" });
            DropIndex("dbo.StudentParent", new[] { "StudentID" });
            DropPrimaryKey("dbo.StudentParent");
            AlterColumn("dbo.StudentParent", "StudentID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.StudentParent", "ParentID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.StudentParent", new[] { "ParentID", "StudentID" });
            RenameColumn(table: "dbo.StudentParent", name: "StudentID", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.StudentParent", name: "ParentID", newName: "StudentID");
            RenameColumn(table: "dbo.StudentParent", name: "__mig_tmp__0", newName: "ParentID");
            CreateIndex("dbo.StudentParent", "StudentID");
            CreateIndex("dbo.StudentParent", "ParentID");
        }
    }
}
