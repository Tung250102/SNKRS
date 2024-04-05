namespace SNKRS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReviewDateTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "DateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reviews", "DateTime");
        }
    }
}
