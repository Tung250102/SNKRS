namespace SNKRS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductVisibility : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "isVisible", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "isVisible");
        }
    }
}
