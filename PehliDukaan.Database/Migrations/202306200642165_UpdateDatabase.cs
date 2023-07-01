namespace PehliDukaan.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDatabase : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "ImageData", c => c.Binary());
            DropColumn("dbo.Categories", "ImageURL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "ImageURL", c => c.String());
            DropColumn("dbo.Categories", "ImageData");
        }
    }
}
