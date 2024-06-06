namespace Electronic_G5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "image_url", c => c.String(unicode: false, storeType: "text"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "image_url", c => c.String(nullable: false, unicode: false, storeType: "text"));
        }
    }
}
