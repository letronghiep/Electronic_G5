namespace Electronic_G5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResetPass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ResetPassToken", c => c.String());
            AddColumn("dbo.Users", "TokenExpiration", c => c.DateTime(nullable: true));

        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "TokenExpiration");
            DropColumn("dbo.Users", "ResetPassToken");
        }
    }
}
