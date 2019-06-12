namespace Events4All.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_attendee_cap : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "AttendeeCap", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Events", "AttendeeCap");
        }
    }
}
