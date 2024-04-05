namespace SNKRS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductSalePrice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "SalePrice", c => c.Decimal(nullable: false, storeType: "money"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "SalePrice");
        }
    }
}
