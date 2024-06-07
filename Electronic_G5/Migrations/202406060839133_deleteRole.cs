namespace Electronic_G5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteRole : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.User", "role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "role", c => c.Int(nullable: false));
        }
    }
}
