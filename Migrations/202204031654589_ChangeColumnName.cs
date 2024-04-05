namespace SNKRS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeColumnName : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ProductCategory", name: "StudentId", newName: "ProductId");
            RenameIndex(table: "dbo.ProductCategory", name: "IX_StudentId", newName: "IX_ProductId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ProductCategory", name: "IX_ProductId", newName: "IX_StudentId");
            RenameColumn(table: "dbo.ProductCategory", name: "ProductId", newName: "StudentId");
        }
    }
}
