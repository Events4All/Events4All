namespace Events4All.DB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailNtfctnSntTimeToPrtcpnts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Participants", "EmailNotificationSentTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Participants", "EmailNotificationSentTime");
        }
    }
}
