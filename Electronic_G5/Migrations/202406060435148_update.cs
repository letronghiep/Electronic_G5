namespace Electronic_G5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.User", "ResetPassToken");
            //DropColumn("dbo.User", "TokenExpiration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "TokenExpiration", c => c.DateTime());
            AddColumn("dbo.Users", "ResetPassToken", c => c.String());
        }
    }
}
