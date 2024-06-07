namespace Electronic_G5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderItem : DbMigration
    {
        public override void Up()
        {
            //DropPrimaryKey("dbo.Order_Items");
            //AlterColumn("dbo.Order_Items", "order_item_id", c => c.Int(nullable: false, identity: true));
            //AddPrimaryKey("dbo.Order_Items", new[] { "order_item_id", "order_id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Order_Items");
            AlterColumn("dbo.Order_Items", "order_item_id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Order_Items", new[] { "order_item_id", "order_id" });
        }
    }
}
