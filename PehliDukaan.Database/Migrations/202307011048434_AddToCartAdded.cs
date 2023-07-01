namespace PehliDukaan.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddToCartAdded : DbMigration
    {
        public override void Up() {
            CreateTable(
                "dbo.CartProducts",
                c => new {
                    Id = c.Int(nullable: false, identity: true),
                    UserId = c.String(),
                    ProductId = c.Int(nullable: false),
                    Quantity = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
        }
    }
}
