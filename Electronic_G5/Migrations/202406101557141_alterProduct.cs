﻿namespace Electronic_G5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "price");
        }
    }
}
