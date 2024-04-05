namespace SNKRS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeProductCategoryRelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Products", new[] { "CategoryId" });
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        StudentId = c.Int(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.StudentId, t.CategoryId })
                .ForeignKey("dbo.Products", t => t.StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.CategoryId);
            
            DropColumn("dbo.Products", "CategoryId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "CategoryId", c => c.Int(nullable: false));
            DropForeignKey("dbo.ProductCategory", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.ProductCategory", "StudentId", "dbo.Products");
            DropIndex("dbo.ProductCategory", new[] { "CategoryId" });
            DropIndex("dbo.ProductCategory", new[] { "StudentId" });
            DropTable("dbo.ProductCategory");
            CreateIndex("dbo.Products", "CategoryId");
            AddForeignKey("dbo.Products", "CategoryId", "dbo.Categories", "Id", cascadeDelete: true);
        }
    }
}
