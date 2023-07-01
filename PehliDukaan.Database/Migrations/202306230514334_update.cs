namespace PehliDukaan.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "ImageData", c => c.Binary());
            AddColumn("dbo.Products", "ImageData", c => c.Binary());
            DropColumn("dbo.Categories", "ImageURL");
            DropColumn("dbo.Products", "ImageURL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ImageURL", c => c.String());
            AddColumn("dbo.Categories", "ImageURL", c => c.String());
            DropColumn("dbo.Products", "ImageData");
            DropColumn("dbo.Categories", "ImageData");
        }
    }
}
