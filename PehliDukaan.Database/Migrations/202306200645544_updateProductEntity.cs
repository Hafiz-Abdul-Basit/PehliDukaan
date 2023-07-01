namespace PehliDukaan.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateProductEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "ImageData", c => c.Binary());
            DropColumn("dbo.Products", "ImageURL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ImageURL", c => c.String());
            DropColumn("dbo.Products", "ImageData");
        }
    }
}
