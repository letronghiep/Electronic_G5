namespace Electronic_G5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResetPass : DbMigration
    {
        public override void Up()
        {

            CreateTable(
                "dbo.Users",
                c => new
                {
                    user_id = c.Int(nullable: false, identity: true),
                    full_name = c.String(nullable: false, maxLength: 100),
                    email = c.String(nullable: false, maxLength: 100),
                    password = c.String(nullable: false, maxLength: 100, unicode: false),
                    address = c.String(nullable: false, maxLength: 100),
                    phone_number = c.String(nullable: false, maxLength: 100, unicode: false),
                    image = c.String(unicode: false, storeType: "text"),
                    role = c.Int(nullable: false),
                    created_at = c.DateTime(precision: 7, storeType: "datetime2"),
                    updated_at = c.DateTime(precision: 7, storeType: "datetime2"),
                    PasswordResetToken = c.String(),
                    TokenExpiration = c.DateTime(),
                    role_id = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.user_id)
                .ForeignKey("dbo.Roles", t => t.role_id)
                .Index(t => t.role_id);
        }
            
            
        public override void Down()
        {
            DropForeignKey("dbo.Users", "role_id", "dbo.Roles");
            DropIndex("dbo.Users", new[] { "role_id" });
            DropTable("dbo.Users");
        }
    }
}
