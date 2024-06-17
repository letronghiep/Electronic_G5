namespace Electronic_G5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCart : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Cart",
            //    c => new
            //        {
            //            cart_id = c.Int(nullable: false, identity: true),
            //            user_id = c.Int(nullable: false),
            //            quantity = c.Int(nullable: false),
            //            product_id = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.cart_id, t.user_id })
            //    .ForeignKey("dbo.Product", t => t.product_id)
            //    .ForeignKey("dbo.User", t => t.user_id)
            //    .Index(t => t.user_id)
            //    .Index(t => t.product_id);
            
            //CreateTable(
            //    "dbo.Product",
            //    c => new
            //        {
            //            product_id = c.Int(nullable: false, identity: true),
            //            product_name = c.String(nullable: false, maxLength: 100),
            //            SKU = c.String(nullable: false, maxLength: 100, unicode: false),
            //            image_url = c.String(unicode: false, storeType: "text"),
            //            description = c.String(maxLength: 500),
            //            stock = c.Int(nullable: false),
            //            price = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            category_id = c.Int(nullable: false),
            //            created_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //            updated_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //        })
            //    .PrimaryKey(t => t.product_id)
            //    .ForeignKey("dbo.Category", t => t.category_id)
            //    .Index(t => t.category_id);
            
            //CreateTable(
            //    "dbo.Category",
            //    c => new
            //        {
            //            category_id = c.Int(nullable: false, identity: true),
            //            category_name = c.String(nullable: false, maxLength: 100),
            //            parent_id = c.Int(),
            //            created_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //            updated_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //        })
            //    .PrimaryKey(t => t.category_id)
            //    .ForeignKey("dbo.Category", t => t.parent_id)
            //    .Index(t => t.parent_id);
            
            //CreateTable(
            //    "dbo.Image",
            //    c => new
            //        {
            //            image_id = c.Int(nullable: false, identity: true),
            //            product_id = c.Int(),
            //            image_url = c.String(maxLength: 255),
            //        })
            //    .PrimaryKey(t => t.image_id)
            //    .ForeignKey("dbo.Product", t => t.product_id)
            //    .Index(t => t.product_id);
            
            //CreateTable(
            //    "dbo.Order_Items",
            //    c => new
            //        {
            //            order_item_id = c.Int(nullable: false, identity: true),
            //            order_id = c.Int(nullable: false),
            //            quantity = c.Int(nullable: false),
            //            price = c.Decimal(nullable: false, precision: 10, scale: 2),
            //            product_id = c.Int(nullable: false),
            //            created_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //            updated_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //        })
            //    .PrimaryKey(t => new { t.order_item_id, t.order_id })
            //    .ForeignKey("dbo.Order", t => t.order_id)
            //    .ForeignKey("dbo.Product", t => t.product_id)
            //    .Index(t => t.order_id)
            //    .Index(t => t.product_id);
            
            //CreateTable(
            //    "dbo.Order",
            //    c => new
            //        {
            //            order_id = c.Int(nullable: false, identity: true),
            //            order_date = c.DateTime(nullable: false),
            //            total_price = c.Decimal(nullable: false, precision: 10, scale: 2),
            //            user_id = c.Int(nullable: false),
            //            order_status = c.String(),
            //            fulfillment_status = c.String(),
            //            payment_id = c.Int(nullable: false),
            //            shipment_id = c.Int(nullable: false),
            //            created_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //            updated_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //        })
            //    .PrimaryKey(t => t.order_id)
            //    .ForeignKey("dbo.Payment", t => t.payment_id)
            //    .ForeignKey("dbo.User", t => t.user_id)
            //    .ForeignKey("dbo.Shipment", t => t.shipment_id)
            //    .Index(t => t.user_id)
            //    .Index(t => t.payment_id)
            //    .Index(t => t.shipment_id);
            
            //CreateTable(
            //    "dbo.Payment",
            //    c => new
            //        {
            //            payment_id = c.Int(nullable: false, identity: true),
            //            payment_date = c.DateTime(nullable: false),
            //            payment_method = c.String(nullable: false, maxLength: 100),
            //            amount = c.Decimal(nullable: false, precision: 10, scale: 2),
            //            user_id = c.Int(nullable: false),
            //            created_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //            updated_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //        })
            //    .PrimaryKey(t => t.payment_id)
            //    .ForeignKey("dbo.User", t => t.user_id)
            //    .Index(t => t.user_id);
            
            //CreateTable(
            //    "dbo.User",
            //    c => new
            //        {
            //            user_id = c.Int(nullable: false, identity: true),
            //            full_name = c.String(nullable: false, maxLength: 100),
            //            email = c.String(nullable: false, maxLength: 100),
            //            password = c.String(nullable: false, maxLength: 100, unicode: false),
            //            address = c.String(nullable: false, maxLength: 100),
            //            phone_number = c.String(nullable: false, maxLength: 100, unicode: false),
            //            image = c.String(unicode: false, storeType: "text"),
            //            created_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //            updated_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //            role_id = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.user_id)
            //    .ForeignKey("dbo.Role", t => t.role_id)
            //    .Index(t => t.role_id);
            
            //CreateTable(
            //    "dbo.Role",
            //    c => new
            //        {
            //            role_id = c.Int(nullable: false, identity: true),
            //            role_name = c.String(maxLength: 50),
            //        })
            //    .PrimaryKey(t => t.role_id);
            
            //CreateTable(
            //    "dbo.Shipment",
            //    c => new
            //        {
            //            shipment_id = c.Int(nullable: false, identity: true),
            //            shipment_date = c.DateTime(nullable: false),
            //            address = c.String(nullable: false, maxLength: 100),
            //            city = c.String(nullable: false, maxLength: 100),
            //            state = c.String(nullable: false, maxLength: 100),
            //            country = c.String(nullable: false, maxLength: 100),
            //            zip_code = c.String(nullable: false, maxLength: 10, unicode: false),
            //            user_id = c.Int(nullable: false),
            //            created_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //            updated_at = c.DateTime(precision: 7, storeType: "datetime2"),
            //        })
            //    .PrimaryKey(t => t.shipment_id)
            //    .ForeignKey("dbo.User", t => t.user_id)
            //    .Index(t => t.user_id);
            
            //CreateTable(
            //    "dbo.ProductOption",
            //    c => new
            //        {
            //            product_option_id = c.Int(nullable: false, identity: true),
            //            product_id = c.Int(nullable: false),
            //            product_option_name = c.String(maxLength: 255, unicode: false),
            //            product_option_value = c.String(maxLength: 255, unicode: false),
            //            product_option_price = c.Decimal(nullable: false, precision: 10, scale: 2),
            //        })
            //    .PrimaryKey(t => t.product_option_id)
            //    .ForeignKey("dbo.Product", t => t.product_id)
            //    .Index(t => t.product_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductOption", "product_id", "dbo.Product");
            DropForeignKey("dbo.Order_Items", "product_id", "dbo.Product");
            DropForeignKey("dbo.Shipment", "user_id", "dbo.User");
            DropForeignKey("dbo.Order", "shipment_id", "dbo.Shipment");
            DropForeignKey("dbo.User", "role_id", "dbo.Role");
            DropForeignKey("dbo.Payment", "user_id", "dbo.User");
            DropForeignKey("dbo.Order", "user_id", "dbo.User");
            DropForeignKey("dbo.Cart", "user_id", "dbo.User");
            DropForeignKey("dbo.Order", "payment_id", "dbo.Payment");
            DropForeignKey("dbo.Order_Items", "order_id", "dbo.Order");
            DropForeignKey("dbo.Image", "product_id", "dbo.Product");
            DropForeignKey("dbo.Product", "category_id", "dbo.Category");
            DropForeignKey("dbo.Category", "parent_id", "dbo.Category");
            DropForeignKey("dbo.Cart", "product_id", "dbo.Product");
            DropIndex("dbo.ProductOption", new[] { "product_id" });
            DropIndex("dbo.Shipment", new[] { "user_id" });
            DropIndex("dbo.User", new[] { "role_id" });
            DropIndex("dbo.Payment", new[] { "user_id" });
            DropIndex("dbo.Order", new[] { "shipment_id" });
            DropIndex("dbo.Order", new[] { "payment_id" });
            DropIndex("dbo.Order", new[] { "user_id" });
            DropIndex("dbo.Order_Items", new[] { "product_id" });
            DropIndex("dbo.Order_Items", new[] { "order_id" });
            DropIndex("dbo.Image", new[] { "product_id" });
            DropIndex("dbo.Category", new[] { "parent_id" });
            DropIndex("dbo.Product", new[] { "category_id" });
            DropIndex("dbo.Cart", new[] { "product_id" });
            DropIndex("dbo.Cart", new[] { "user_id" });
            DropTable("dbo.ProductOption");
            DropTable("dbo.Shipment");
            DropTable("dbo.Role");
            DropTable("dbo.User");
            DropTable("dbo.Payment");
            DropTable("dbo.Order");
            DropTable("dbo.Order_Items");
            DropTable("dbo.Image");
            DropTable("dbo.Category");
            DropTable("dbo.Product");
            DropTable("dbo.Cart");
        }
    }
}
