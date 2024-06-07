namespace Electronic_G5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DelRole : DbMigration
    {
        public override void Up()
        {
          //  DropColumn("dbo.User", "role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "role", c => c.Int(nullable: false));
        }
    }
}
