namespace Electronic_G5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterUser : DbMigration
    {
        public override void Up()
        {

            //DropColumn("dbo.User", "ResetPassToken");
            //DropColumn("dbo.User", "TokenExpiration");


            //DropColumn("dbo.User", "PasswordResetToken");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ResetPassToken");
            DropColumn("dbo.Users", "TokenExpiration");


        }
    }
}
