namespace Events4All.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotificationFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Participants", "emailNotificationOn", c => c.Boolean(nullable: false));
            AddColumn("dbo.Participants", "SMSNotificationOn", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Participants", "SMSNotificationOn");
            DropColumn("dbo.Participants", "emailNotificationOn");
        }
    }
}
