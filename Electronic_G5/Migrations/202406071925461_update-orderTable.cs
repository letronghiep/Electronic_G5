namespace Electronic_G5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateorderTable : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Order", "order_status", c => c.String());
            //DropColumn("dbo.Order", "payment_status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Order", "payment_status", c => c.String());
            DropColumn("dbo.Order", "order_status");
        }
    }
}
