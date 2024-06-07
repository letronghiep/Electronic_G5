namespace Electronic_G5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTable : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Carts", newName: "Cart");
            RenameTable(name: "dbo.Products", newName: "Product");
            RenameTable(name: "dbo.Categories", newName: "Category");
            RenameTable(name: "dbo.Images", newName: "Image");
            RenameTable(name: "dbo.Orders", newName: "Order");
            RenameTable(name: "dbo.Payments", newName: "Payment");
            RenameTable(name: "dbo.Users", newName: "User");
            RenameTable(name: "dbo.Roles", newName: "Role");
            RenameTable(name: "dbo.Shipments", newName: "Shipment");
            RenameTable(name: "dbo.ProductOptions", newName: "ProductOption");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.ProductOption", newName: "ProductOptions");
            RenameTable(name: "dbo.Shipment", newName: "Shipments");
            RenameTable(name: "dbo.Role", newName: "Roles");
            RenameTable(name: "dbo.User", newName: "Users");
            RenameTable(name: "dbo.Payment", newName: "Payments");
            RenameTable(name: "dbo.Order", newName: "Orders");
            RenameTable(name: "dbo.Image", newName: "Images");
            RenameTable(name: "dbo.Category", newName: "Categories");
            RenameTable(name: "dbo.Product", newName: "Products");
            RenameTable(name: "dbo.Cart", newName: "Carts");
        }
    }
}
